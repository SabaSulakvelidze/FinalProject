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
        IMapper mapper,
        INotificationService notifications
        ) : IProjectTaskService
    {
        public async Task<ProjectTaskResponse> CreateProjectTask(CreateProjectTaskRequest request)
        {
            _ = await context.Projects.FindAsync(request.ProjectId)
                ?? throw new ElementNotFoundException($"Project with id {request.ProjectId} was not found");

            _ = await context.Users.FindAsync(request.TaskAssigneeId)
                ?? throw new ElementNotFoundException($"Emplyee with id {request.TaskAssigneeId} was not found");

            var isMember = await context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == request.ProjectId && pm.MemberId == request.TaskAssigneeId);

            if (!isMember)
                throw new ElementNotFoundException($"Emplyee with id {request.TaskAssigneeId} is not a member of project with id {request.ProjectId}");

            var projectTask = mapper.Map<ProjectTask>(request);
            projectTask.CreatedAt = DateTime.Now;
            projectTask.TaskIssuerId = currentUser.UserId;
            projectTask.TaskStatus = "TODO";

            context.ProjectTasks.Add(projectTask);
            await context.SaveChangesAsync();

            var taskWithRelations = await context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.TaskAssignee)
                .Include(t => t.TaskIssuer)
                .FirstAsync(t => t.Id == projectTask.Id);

            var response = mapper.Map<ProjectTaskResponse>(taskWithRelations);

            await notifications.TaskCreatedOrUpdatedAsync(taskWithRelations.TaskAssigneeId, response);

            return response;
        }

        public async Task DeleteProjectTask(int id)
        {
            var projectTask = await context.ProjectTasks.FindAsync(id)
                 ?? throw new ElementNotFoundException($"ProjectTask with id {id} was not found");

            context.ProjectTasks.Remove(projectTask);
            await context.SaveChangesAsync();
        }

        public async Task<List<ProjectTaskResponse>> GetMyProjcetTasks()
        {
            var myTasks = await context.ProjectTasks
                .Where(pt => pt.TaskAssigneeId == currentUser.UserId)
                .Include(t => t.Project)
                .Include(t => t.TaskAssignee)
                .Include(t => t.TaskIssuer)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return mapper.Map<List<ProjectTaskResponse>>(myTasks);
        }

        public async Task<ProjectTaskResponse> GetProjectTaskById(int id)
        {
            var projectTask = await context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.TaskAssignee)
                .Include(t => t.TaskIssuer)
                .FirstOrDefaultAsync(t => t.Id == id)
                ?? throw new ElementNotFoundException($"ProjectTask with id {id} was not found");

            return mapper.Map<ProjectTaskResponse>(projectTask);
        }

        public async Task<List<ProjectTaskResponse>> GetTasksByProject(int projectId)
        {
            if (!await context.Projects.AnyAsync(p => p.Id == projectId))
                throw new ElementNotFoundException($"Project with id {projectId} was not found");

            var projectTasks = await context.ProjectTasks
                .Where(pt => pt.ProjectId == projectId)
                .Include(t => t.Project)
                .Include(t => t.TaskAssignee)
                .Include(t => t.TaskIssuer)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return mapper.Map<List<ProjectTaskResponse>>(projectTasks);
        }

        public async Task<ProjectTaskResponse> UpdateProjectTask(int id, UpdateProjectTaskRequest request)
        {
            var projectTask = await context.ProjectTasks.FindAsync(id)
                 ?? throw new ElementNotFoundException($"ProjectTask with id {id} was not found");

            _ = await context.Projects.FindAsync(request.ProjectId)
                ?? throw new ElementNotFoundException($"Project with id {request.ProjectId} was not found");

            _ = await context.Users.FindAsync(request.TaskAssigneeId)
                ?? throw new ElementNotFoundException($"Emplyee with id {request.TaskAssigneeId} was not found");

            var isMember = await context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == request.ProjectId && pm.MemberId == request.TaskAssigneeId);

            if (!isMember)
                throw new ElementNotFoundException($"Emplyee with id {request.TaskAssigneeId} is not a member of project with id {request.ProjectId}");

            mapper.Map(request, projectTask);
            await context.SaveChangesAsync();

            var taskWithRelations = await context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.TaskAssignee)
                .Include(t => t.TaskIssuer)
                .FirstAsync(t => t.Id == projectTask.Id);

            var response = mapper.Map<ProjectTaskResponse>(taskWithRelations);

            await notifications.TaskCreatedOrUpdatedAsync(taskWithRelations.TaskAssigneeId, response);

            return response;
        }
    }
}