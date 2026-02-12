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
    public class ProjcetTaskController(IProjectTaskService service) : ControllerBase
    {
        [HttpGet("~/api/GetTasksByProject/{projectId}")]
        [Authorize]
        public async Task<ActionResult<List<ProjectTaskResponse>>> GetTasksByProject(int projectId)
        {
            return Ok(await service.GetTasksByProject(projectId));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectTaskResponse>> CreateProjectTask(CreateProjectTaskRequest request)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER") || permissions.Contains("EMPLOYEE")))
                return Forbid();

            if (request == null)
                return BadRequest(request);

            return Ok(await service.CreateProjectTask(request));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectTaskResponse>> GetProjectTaskById(int id)
        {
            return Ok(await service.GetProjectTaskById(id));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectTaskResponse>> UpdateProjectTask(int id, UpdateProjectTaskRequest request)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER") || permissions.Contains("EMPLOYEE")))
                return Forbid();

            if (request == null)
                return BadRequest(request);

            return Ok(await service.UpdateProjectTask(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProjectTask(int id)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!(permissions.Contains("ADMIN") || permissions.Contains("MANAGER")))
                return Forbid();
          
            await service.DeleteProjectTask(id);
            return Ok($"ProjectTask with id {id} was deleted!");
        }
    }
}
