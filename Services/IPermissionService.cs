using FinalProject.Models.Requests;
using FinalProject.Models.Responses;

namespace FinalProject.Services
{
    public interface IPermissionService
    {
        Task<PermissionResponse> CreatePermission(PermissionRequest request);
        Task<List<PermissionResponse>> GetAllPermissions();
        Task<PermissionResponse> GetPermissionById(int id);
        Task<PermissionResponse> UpdatePermission(int id, PermissionRequest request);
        Task RemovePermissionForUser(int userId, int permissionId);
        Task DeletePermission(int id);
        Task AssignPermissionToUser(int userId, int permissionId);
    }
}
