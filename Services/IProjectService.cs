using FinalProject.Models.Requests;
using FinalProject.Models.Responses;

namespace FinalProject.Services
{
    public interface IProjectService
    {
        Task<List<ProjectResponse>> GetAllProjects();

        Task<ProjectResponse> GetProjectById(int id);

        Task<ProjectResponse> CreateProject(CreateProjectRequest request);

        Task<ProjectResponse> UpdateProject(int id, UpdateProjectRequest request);

        Task DeleteProject(int id);

        Task<List<UserResponse>> AddMembersToProject(int projectId, List<int> ids);

        Task RemoveMembersFromProject(int projectId, List<int> ids);

        Task<List<UserResponse>> GetProjectMembers(int projectId);


    }
}
