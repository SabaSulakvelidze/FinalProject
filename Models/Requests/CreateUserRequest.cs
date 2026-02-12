using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "მომხმარებლის სახელი სავალდებულოა")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "მომხმარებლის სახელი უნდა იყოს 3–25 სიმბოლო")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "მომხმარებლის სახელი შეიცავს დაუშვებელ სიმბოლოებს")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "პაროლი სავალდებულოა")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "პაროლი უნდა იყოს მინიმუმ 6 სიმბოლო")]
        public string Password { get; set; } = null!;
    }
}
