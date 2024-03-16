using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;

namespace JobDone.ViewModels
{
    public class SignUpCustomerViewModel
    {
        public CustomerModel? Customer { get; set; }
        public List<SecurityQuestionModel>? SecurityQuestions {  get; set; }
    }
}
