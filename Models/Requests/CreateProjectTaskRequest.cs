namespace FinalProject.Models.Requests
{
    public class CreateProjectTaskRequest
    {
        public string TaskName { get; set; } = null!;

        public string TaskDescription { get; set; } = null!;

        public int ProjectId { get; set; }

        public int TaskIssuerId { get; set; }

        public int TaskAssigneeId { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
