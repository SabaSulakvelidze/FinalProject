using System.Security.Claims;

namespace FinalProject.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        
    }
}
