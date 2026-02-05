using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController(IPermissionsService permissionService) : ControllerBase
    {

        [HttpGet]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<List<Permission>>> GetAllPermissions()
        {
            return Ok(await permissionService.GetAllPermissions());
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<PermissionResponse>> CreatePermission(PermissionRequest request)
        {
            if (request == null) 
                return BadRequest(request);

            return Ok(await permissionService.CreatePermission(request));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<PermissionResponse>> GetPermissionById(int id)
        {
            var result = await permissionService.GetPermissionById(id);
            if (result == null) return NotFound($"Permission with id {id} not found");
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<PermissionResponse>> UpdatePermission(int id, PermissionRequest request)
        {
            if(await permissionService.GetPermissionById(id)==null)
            return BadRequest($"Permission with id {id} not found");

            var result = await permissionService.GetAllPermissions();
            if (result.Any(p => p.PermissionName == request.PermissionName))
                return Conflict($"Permission with name '{request.PermissionName}' already exists");
            return Ok(await permissionService.UpdatePermission(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult> DeletePermission(int id)
        {
            await permissionService.DeletePermission(id);
            return Ok($"Permission with id {id} was deleted!");
        }

        [HttpPost("~/api/AssignPermissionToUser")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult> AssignPermissionToUser(int userId,int permissionId)
        {
            await permissionService.AssignPermissionToUser(userId, permissionId);
            return Ok($"Permission with id {permissionId} has been assigned to User with Id {userId} successfully!");
        }

        [HttpPut("~/api/RemovePermissionForUser")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult> RemovePermissionForUser(int userId, int permissionId)
        {
            await permissionService.RemovePermissionForUser(userId, permissionId);
            return Ok($"Permission with id {permissionId} has been removed from User with Id {userId} successfully!");
        }

    }
}
