namespace FinalProject.Models.Responses
{
    public class ProjectResponse
    {
        public int Id { get; set; }

        public string ProjectName { get; set; } = null!;

        public int ProjectManagerId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
