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
        public async Task<ActionResult<UserResponse>> Register(CreateUserRequest userRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await userServices.CreateUser(userRequest));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var result = await userServices.GetUserById(id);

            if (result == null) return BadRequest($"User with id {id} not found"); 

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> GetAllUsers()
        {
            return Ok(await userServices.GetAllUsers());
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id,UpdateUserRequest request)
        {
            return Ok(await userServices.UpdateUser(id, request));
        }

        

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            await userServices.DeleteUser(id);
            return Ok();
        }


    }
}
