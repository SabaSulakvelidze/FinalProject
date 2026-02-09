using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class PermissionRequest
    {
        [Required(ErrorMessage = "უფლების სახელი სავალდებულოა")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "უფლების სახელი უნდა იყოს 3–50 სიმბოლო")]
        [RegularExpression(@"^[A-Z_]+$", ErrorMessage = "გამოიყენე მხოლოდ დიდი ასოები და ქვედა ტირე (_)")]
        public string PermissionName { get; set; } = null!;
    }
}
