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
    public class UserController(UserServices userServices) : ControllerBase
    {
        /*private readonly AuthService _authService = authService;
        private readonly UserServices _userServices = userServices;*/

        [HttpPost("/api/SignIn")]
        public async Task<ActionResult> SignIn(LogInRequest logInRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
           
            return Ok(await userServices.SignIn(logInRequest));
        }

        [HttpPost("/api/Register")]
        public async Task<ActionResult<UserResponse>> Register(UserRequest userRequest)
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
        [Authorize(Policy = "RequireManagerPermission")]
        public async Task<ActionResult<UserResponse>> GetAllUsers()
        {
            return Ok(await userServices.GetAllUsers());
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminPermission")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id,UserRequest userRequest)
        {
            return Ok(await userServices.UpdateUser(id, userRequest));
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
