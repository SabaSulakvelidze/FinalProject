using System.Security.Claims;

namespace FinalProject.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId =>
         int.Parse(
             new HttpContextAccessor().HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? throw new UnauthorizedAccessException()
         );
    }
}
