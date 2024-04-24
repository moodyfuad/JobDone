using JobDone.Validations;
using JobDone.Models.SecurityQuestions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JobDone.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [StringLength(50)]
        [Required]
        [Remote(action: "IsPasswordOk", "Validation")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(otherProperty: "Password", ErrorMessage = "Passwords Does Not Match")]
        public string? ConfirmPassword { get; set; }
        public List<SecurityQuestionModel>? SecurityQuestions { get; set; }
    }
}
