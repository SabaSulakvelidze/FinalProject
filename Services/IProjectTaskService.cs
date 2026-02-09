using FinalProject.Models.Requests;
using FinalProject.Models.Responses;

namespace FinalProject.Services
{
    public interface IProjectTaskService
    {
        Task<List<ProjectTaskResponse>> GetTasksByProject(int projectId);

        Task<ProjectTaskResponse> GetProjectTaskById(int id);

        Task<ProjectTaskResponse> CreateProjectTask(CreateProjectTaskRequest request);

        Task<ProjectTaskResponse> UpdateProjectTask(int id, UpdateProjectTaskRequest request);

        Task DeleteProjectTask(int id);
    }
}
