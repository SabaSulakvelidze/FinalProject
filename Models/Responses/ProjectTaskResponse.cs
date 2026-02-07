namespace FinalProject.Models.Responses
{
    public class ProjectTaskResponse
    {
        public int Id { get; set; }

        public string TaskName { get; set; } = null!;

        public string TaskDescription { get; set; } = null!;

        public string TaskStatus { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime DeadLine { get; set; }

        public ProjectResponse Project { get; set; } = null!;

        public UserResponse TaskAssignee { get; set; } = null!;

        public UserResponse TaskIssuer { get; set; } = null!;
    }
}
