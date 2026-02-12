using Azure.Core;
using FinalProject.Models;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kindergarten.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices) : ControllerBase
    {
        [HttpPost("/api/SignIn")]
        public async Task<ActionResult> SignIn(LogInRequest logInRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
           
            return Ok(await userServices.SignIn(logInRequest));
        }

        [HttpPost("/api/Register")]
        public async Task<ActionResult<UserResponse>> Register(CreateUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (request == null)
                return BadRequest(request);

            return Ok(await userServices.CreateUser(request));
        }

        [HttpGet("{id}")]
        [Authorize]
        //[Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            return Ok(await userServices.GetUserById(id));
        }

        [HttpGet]
        [Authorize]
        //[Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> GetAllUsers()
        {
            return Ok(await userServices.GetAllUsers());
        }

        [HttpPut("{id}")]
        [Authorize]
        //[Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id,UpdateUserRequest request)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!permissions.Contains("ADMIN")) return Forbid();

            if (request == null)
                return BadRequest(request);

            return Ok(await userServices.UpdateUser(id, request));
        }

        

        [HttpDelete("{id}")]
        [Authorize]
        //[Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            var permissions = User.Claims.Where(i => i.Type == "Permission").Select(i => i.Value).ToList();
            if (!permissions.Contains("ADMIN")) return Forbid();

            await userServices.DeleteUser(id);
            return Ok();
        }


    }
}
