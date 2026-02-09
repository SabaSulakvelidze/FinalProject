using AutoMapper;
using FinalProject.Exceptions;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public class ProjectService(
        AlgoUniFinalProjectDbContext context,
        IMapper mapper
        ) : IProjectService
    {

        public async Task<ProjectResponse> CreateProject(CreateProjectRequest request)
        {
            var project = mapper.Map<Project>(request);
            project.CreatedAt = DateTime.UtcNow;

            context.Projects.Add(project);

            await context.SaveChangesAsync();

            return mapper.Map<ProjectResponse>(project);
        }

        public async Task DeleteProject(int id)
        {
            var project = await context.Projects.FindAsync(id)
                 ?? throw new ElementNotFoundException($"Project with id {id} was not found");

            var memberIds = await context.ProjectMembers
                .Where(p => p.ProjectId == id)
                .Select(p=>p.MemberId)
                .ToListAsync();

            if (memberIds.Count != 0)
                throw new ConflictException($"Project with id {id} has members: {string.Join(", ", memberIds)}");

            context.Projects.Remove(project);

            await context.SaveChangesAsync();
        }

        public async Task<List<ProjectResponse>> GetAllProjects()
        {
            return mapper.Map<List<ProjectResponse>>(await context.Projects.ToListAsync());
        }

        public async Task<ProjectResponse> GetProjectById(int id)
        {
            var project = await context.Projects.FindAsync(id)
                ?? throw new ElementNotFoundException($"Project with id {id} was not found");
            return mapper.Map<ProjectResponse>(project);
        }

        public async Task<List<UserResponse>> GetProjectMembers(int projectId)
        {
            var projectExists = await context.Projects.AnyAsync(p => p.Id == projectId);
            if (!projectExists)
                throw new ElementNotFoundException($"Project with id {projectId} was not found");

            var projectMembers = await context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId)
                .Select(pm => pm.Member)
                .ToListAsync();

            return mapper.Map<List<UserResponse>>(projectMembers);
        }

        public async Task<ProjectResponse> UpdateProject(int id, UpdateProjectRequest request)
        {
            var project = await context.Projects.FindAsync(id)
                ?? throw new ElementNotFoundException($"Project with id {id} was not found");

            mapper.Map(request, project);

            await context.SaveChangesAsync();
            return mapper.Map<ProjectResponse>(project);
        }

        public async Task<List<UserResponse>> AddMembersToProject(int projectId, List<int> ids)
        {
            var projectExists = await context.Projects.AnyAsync(p => p.Id == projectId);
            if (!projectExists)
                throw new ElementNotFoundException($"Project with id {projectId} was not found");

            var existingUsers = await context.Users
                .Where(u => ids.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();

            var missingIds = ids.Except(existingUsers).ToList();
            if (missingIds.Count != 0)
                throw new ElementNotFoundException($"Users with ids: {string.Join(", ",missingIds)} were not found");
            
            var projectMembersIds = await context.ProjectMembers
                .Where(pm => ids.Contains(pm.MemberId))
                .Select(pm => pm.MemberId)
                .ToListAsync();

            if (projectMembersIds.Count != 0)
                throw new ConflictException($"These users are already members. User ids: {string.Join(", ", projectMembersIds)}");

            foreach (var item in existingUsers)
            {
                context.ProjectMembers.Add(new() { ProjectId = projectId, MemberId = item});
            }

            await context.SaveChangesAsync();

            var members = await context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId)
                .Include(pm=>pm.Member)
                    .ThenInclude(m=>m.PermissionsForUsers)
                        .ThenInclude(pfu => pfu.Permission)
                .Select(pm=>pm.Member)
                .ToListAsync();
            return mapper.Map<List<UserResponse>>(members);
        }

        public async Task RemoveMembersFromProject(int projectId, List<int> ids)
        {
            var projectExists = await context.Projects.AnyAsync(p => p.Id == projectId);
            if (!projectExists)
                throw new ElementNotFoundException($"Project with id {projectId} was not found");

            var existingUsers = await context.Users
                .Where(u => ids.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();

            var missingIds = ids.Except(existingUsers).ToList();
            if (missingIds.Count != 0)
                throw new ElementNotFoundException($"Users with ids: {string.Join(", ", missingIds)} was not found");

            var projectMembers = await context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId && ids.Contains(pm.MemberId))
                .ToListAsync();

            if (projectMembers.Count == 0)
                throw new ElementNotFoundException(
                    $"None of the specified users are members of project {projectId}");

            context.ProjectMembers.RemoveRange(projectMembers);
            await context.SaveChangesAsync();

        }
    }
}
