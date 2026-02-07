namespace FinalProject.Models.Requests
{
    public class CreateProjectRequest
    {
        public string ProjectName { get; set; } = null!;

        public int ProjectManagerId { get; set; }
    }
}
