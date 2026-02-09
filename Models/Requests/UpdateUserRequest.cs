using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "მომხმარებლის სახელი სავალდებულოა")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "მომხმარებლის სახელი უნდა იყოს 3–25 სიმბოლო")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "მომხმარებლის სახელი შეიცავს დაუშვებელ სიმბოლოებს")]
        public string Username { get; set; } = null!;
    }
}
