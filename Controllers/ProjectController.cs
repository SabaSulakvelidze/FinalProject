using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IProjectService service) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ProjectResponse>>> GetAllProjects()
        {
            return Ok(await service.GetAllProjects());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectResponse>> CreateProject(CreateProjectRequest request)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER")))
                return Forbid();

            if (request == null)
                return BadRequest(request);

            return Ok(await service.CreateProject(request));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectResponse>> GetProjectById(int id)
        {
            var result = await service.GetProjectById(id);
            if (result == null) return NotFound($"Project with id {id} not found");
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectResponse>> UpdateProject(int id, UpdateProjectRequest request)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER")))
                return Forbid();

            if (await service.GetProjectById(id) == null)
                return BadRequest($"Project with id {id} not found");

            var result = await service.GetAllProjects();
            if (result.Any(p => p.ProjectName == request.ProjectName))
                return Conflict($"Project with name '{request.ProjectName}' already exists");
            return Ok(await service.UpdateProject(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER")))
                return Forbid();

            var result = await service.GetProjectById(id);
            if (result == null) return NotFound($"Project with id {id} not found");
            await service.DeleteProject(id);
            return Ok($"Permission with id {id} was deleted!");
        }

        [HttpPost("~/api/GetProjectMembers/{projectId}")]
        [Authorize]
        public async Task<ActionResult<List<UserResponse>>> GetProjectMembers(int projectId)
        {
            var result = await service.GetProjectById(projectId);
            if (result == null) return NotFound($"Project with id {projectId} not found");
            return Ok(await service.GetProjectMembers(projectId));
        }

        [HttpPut("~/api/AddMembersToProject/{projectId}")]
        [Authorize]
        public async Task<ActionResult<List<UserResponse>>> AddMembersToProject(int projectId, List<int> ids)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER")))
                return Forbid();

            var result = await service.GetProjectById(projectId);
            if (result == null) return NotFound($"Project with id {projectId} not found");
            return Ok(await service.AddMembersToProject(projectId, ids));
        }

        [HttpPut("~/api/RemoveMembersFromProject/{projectId}")]
        [Authorize]
        public async Task<ActionResult> RemoveMembersFromProject(int projectId, List<int> ids)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER")))
                return Forbid();

            var result = await service.GetProjectById(projectId);
            if (result == null) return NotFound($"Project with id {projectId} not found");
            await service.RemoveMembersFromProject(projectId, ids);
            return Ok($"Users with ids: {string.Join(", ",ids)}, has been removed form Project with id {projectId}");
        }
    }
}
