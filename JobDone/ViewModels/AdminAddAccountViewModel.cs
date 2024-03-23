using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JobDone.ViewModels
{
    public class AdminAddAccountViewModel
    {
        [StringLength(50)]
        [Required]
        [Remote(action: "ValidateFirstName", controller: "Validation")]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        [Required]
        [Remote(action: "ValidateLastName", controller: "Validation")]
        public string LastName { get; set; } = null!;

        [StringLength(50)]
        [Required]
        [MinLength(3, ErrorMessage = "Username Can Not Be Less Than 3 Characters")]
        [Remote(action: "IsUsernameExist", controller: "Admin")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Format Use [example@mail.com]")]
        public string Email { get; set; } = null!;

        [StringLength(50)]
        [Required]
        [Remote(action: "IsPasswordOk", "Validation")]
        public string Password { get; set; } = null!;


        [Required]
        [Compare(otherProperty: "Password", ErrorMessage = "Passwords Does Not Match")]
        public string? ConfirmPassword { get; set; }




    }
}
