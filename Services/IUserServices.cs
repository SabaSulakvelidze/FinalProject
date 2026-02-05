using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
namespace FinalProject.Services
{
    public interface IUserServices
    {
        Task<UserResponse> CreateUser(CreateUserRequest request);

        Task<List<UserResponse>> GetAllUsers();

        Task<UserResponse> GetUserById(int id);

        Task<UserResponse> UpdateUser(int id, UpdateUserRequest request);

        Task DeleteUser(int id);

        Task<string> SignIn(LogInRequest logInRequest);
    }
}
