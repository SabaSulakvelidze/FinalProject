using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class LogInRequest
    {
        [Required(ErrorMessage = "მომხმარებლის სახელი სავალდებულოა")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "სახელის სიგრძე უნდა იყოს 3–25 სიმბოლო")]
        [RegularExpression(@"\S+", ErrorMessage = "სახელი არ შეიძლება შეიცავდეს მხოლოდ ცარიელ სივრცეებს")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "პაროლი სავალდებულოა")]
        [StringLength(128, ErrorMessage = "პაროლი ძალიან გრძელია")]
        public string Password { get; set; } = null!;

    }
}
