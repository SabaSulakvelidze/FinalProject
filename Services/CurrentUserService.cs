using System.Security.Claims;

namespace FinalProject.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
       
        public int UserId =>
            int.Parse(
                httpContextAccessor.HttpContext?.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("UserId claim missing")
            );
    }
}
