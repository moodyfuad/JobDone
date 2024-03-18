using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobDone.ViewModels
{
    public class SignUpCustomerViewModel
    {
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        [Required]
        public string LastName { get; set; } = null!;

        [StringLength(50)]
        [Required]
        [MinLength(3, ErrorMessage = "Username Can Not Be Less Than 3 Characters")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Format Use [example@mail.com]")]
        public string Email { get; set; } = null!;

        [StringLength(50)]
        [Required]
        public string Password { get; set; } = null!;
        
        
        [Required]
        [Compare(otherProperty: "Password", ErrorMessage = "Passwords Does Not Match")]
        public string? ConfirmPassword { get; set; }

        [StringLength(10)]
        [Required]
        public string Gender { get; set; } = null!;

        [StringLength(20)]
        [Required]
        public string PhoneNumber { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public byte[]? ProfilePicture { get; set; } = null!;

        public IFormFile? profilePictureAsFile { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage ="Please answer the security question")]
        public string SecurityQuestionAnswer { get; set; } = null!;

        [Required(ErrorMessage ="Please Select Security Question")]
        public int SecurityQuestionIdFk { get; set; }


        [Required(ErrorMessage ="You Must Accept The Privacy Policy")]
        public bool Accept {  get; set; }

        public List<SecurityQuestionModel>? SecurityQuestions {  get; set; }
    }
}
