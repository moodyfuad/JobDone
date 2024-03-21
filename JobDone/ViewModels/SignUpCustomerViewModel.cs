using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using JobDone.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobDone.ViewModels
{
    public class SignUpCustomerViewModel
    {
      

        [StringLength(50)]
        [Required]        
        [Remote(action: "ValidateFirstName", controller:"Validation")]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        [Required]
        [Remote(action: "ValidateLastName", controller:"Validation")]
        public string LastName { get; set; } = null!;

        [StringLength(50)]
        [Required]
        [MinLength(3, ErrorMessage = "Username Can Not Be Less Than 3 Characters")]
        [Remote(action:"IsUsernameExist",controller:"Customer")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Format Use [example@mail.com]")]
        public string Email { get; set; } = null!;

        [StringLength(50)]
        [Required]
        [Remote(action:"IsPasswordOk","Validation")]
        public string Password { get; set; } = null!;
        
        
        [Required]
        [Compare(otherProperty: "Password", ErrorMessage = "Passwords Does Not Match")]
        public string? ConfirmPassword { get; set; }

        [StringLength(10)]
        [Required]
        public string Gender { get; set; } = null!;

        [Length(8,13,ErrorMessage ="Invalid Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [StringLength(20)]
        [Required]
        public string PhoneNumber { get; set; } = null!;

        [BirthDateValidate]
        public DateOnly BirthDate { get; set; }

        public byte[]? ProfilePicture { get; set; } = null!;

        public IFormFile? profilePictureAsFile { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage ="Please answer the security question")]
        public string SecurityQuestionAnswer { get; set; } = null!;

        //[Required(ErrorMessage ="Please Select Security Question")]
        [SecurityQuestionIdValidate]
        public int SecurityQuestionIdFk { get; set; }


        [Required(ErrorMessage ="You Must Accept The Privacy Policy")]
        public bool Accept {  get; set; }

        public List<SecurityQuestionModel>? SecurityQuestions {  get; set; }
    }
}
