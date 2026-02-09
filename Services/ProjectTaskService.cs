using AutoMapper;
using FinalProject.Exceptions;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public class ProjectTaskService(
        AlgoUniFinalProjectDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper
        ) : IProjectTaskService
    {
        public async Task<ProjectTaskResponse> CreateProjectTask(CreateProjectTaskRequest request)
        {
            var project = await context.Projects.FindAsync(request.ProjectId)
                ?? throw new ElementNotFoundException($"Project with id {request.ProjectId} was not found");

            var projectTask = mapper.Map<ProjectTask>(request);
            projectTask.CreatedAt = DateTime.Now;
            projectTask.TaskIssuerId = currentUser.UserId;

            context.ProjectTasks.Add(projectTask);

            await context.SaveChangesAsync();

            return mapper.Map<ProjectTaskResponse>(projectTask);
        }

        public async Task DeleteProjectTask(int id)
        {
            var projectTask = await context.ProjectTasks.FindAsync(id)
                 ?? throw new ElementNotFoundException($"ProjectTask with id {id} was not found");

            context.ProjectTasks.Remove(projectTask);

            await context.SaveChangesAsync();
        }

        public async Task<ProjectTaskResponse> GetProjectTaskById(int id)
        {
            var projectTask = await context.ProjectTasks.FindAsync(id)
                ?? throw new ElementNotFoundException($"ProjectTask with id {id} was not found");
            return mapper.Map<ProjectTaskResponse>(projectTask);
        }

        public async Task<List<ProjectTaskResponse>> GetTasksByProject(int projectId)
        {
            var projectTasks = await context.ProjectTasks
                .Where(pt => pt.ProjectId == projectId)
                .ToListAsync();
            return mapper.Map<List<ProjectTaskResponse>>(projectTasks);
        }

        public async Task<ProjectTaskResponse> UpdateProjectTask(int id, UpdateProjectTaskRequest request)
        {
            var projectTasks = await context.ProjectTasks.FindAsync(id)
                 ?? throw new ElementNotFoundException($"ProjectTask with id {id} was not found");

            mapper.Map(request, projectTasks);

            await context.SaveChangesAsync();
            return mapper.Map<ProjectTaskResponse>(projectTasks);
        }
    }
}
