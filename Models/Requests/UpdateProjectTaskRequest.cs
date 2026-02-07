namespace FinalProject.Models.Requests
{
    public class UpdateProjectTaskRequest
    {
        public string TaskName { get; set; } = null!;

        public string TaskDescription { get; set; } = null!;

        public int ProjectId { get; set; }

        public int TaskAssigneeId { get; set; }

        public string TaskStatus { get; set; } = null!;

        public DateTime DeadLine { get; set; }
    }
}
