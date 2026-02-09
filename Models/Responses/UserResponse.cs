namespace FinalProject.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;

        public List<string> PermissionNames { get; set; } = [];
    }
}
