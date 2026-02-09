using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class CreateProjectRequest
    {
        [Required(ErrorMessage = "პროექტის სახელი სავალდებულოა")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "პროექტის სახელი უნდა იყოს 3–100 სიმბოლო")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_]+$", ErrorMessage = "პროექტის სახელი შეიცავს დაუშვებელ სიმბოლოებს")]
        public string ProjectName { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "პროექტის მენეჯერის აიდი არასწორია")]
        public int ProjectManagerId { get; set; }
    }
}
