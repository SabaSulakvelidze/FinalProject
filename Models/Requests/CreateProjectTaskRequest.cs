using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class CreateProjectTaskRequest
    {
        [Required(ErrorMessage = "დავალების სახელი სავალდებულოა")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "დავალების სახელი უნდა იყოს 3–150 სიმბოლო")]
        public string TaskName { get; set; } = null!;
        [StringLength(1000, ErrorMessage = "დავალების აღწერა ძალიან გრძელია")]
        public string TaskDescription { get; set; } = null!;
        [Range(1, int.MaxValue, ErrorMessage = "პროექტის აიდი არასწორია")]
        public int ProjectId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "დავალების შემსრულებლის აიდი არასწორია")]
        public int TaskAssigneeId { get; set; }
        [Required(ErrorMessage = "დედლაინი სავალდებულოა")]
        public DateTime DeadLine { get; set; }
    }
}
