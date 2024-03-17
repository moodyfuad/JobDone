using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using System.ComponentModel.DataAnnotations;

namespace JobDone.ViewModels
{
    public class SignUpCustomerViewModel
    {
        public CustomerModel? Customer { get; set; }
        public List<SecurityQuestionModel>? SecurityQuestions {  get; set; }

        [Required(ErrorMessage ="You Must Accept The Privacy Policy")]
        public bool Accept {  get; set; }

        [Required]
/*        [Compare(otherProperty: "Customer.Password", ErrorMessage ="Passwords Does Not Match")]
*/      public string? ConfirmPassword { get; set; }
        public IFormFile? profilePicture { get; set; }
    }
}
