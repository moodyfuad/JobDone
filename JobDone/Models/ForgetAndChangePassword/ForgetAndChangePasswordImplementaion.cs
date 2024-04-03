using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.ForgetAndChangePassword
{
    public class ForgetAndChangePasswordImplementaion : IForgetAndChanePassword
    {
        private readonly DbSet<SellerModel> _seller;
        private readonly DbSet<CustomerModel> _customer;
        private readonly DbSet<SecurityQuestionModel> _questions;
        private readonly JobDoneContext _context;

        public ForgetAndChangePasswordImplementaion(JobDoneContext context)
        {
            _seller = context.SellerModels;
            _customer = context.CustomerModels;
            _questions = context.SecurityQuestionModels;
            _context = context;
        }

        public bool ConfirmTheAnswerForTheCustomer(string username, int questionId, string answer)
        {
            if (answer != null && username != null && questionId != 0)
            {
                var result = _customer
                   .Where(x => x.Username == username || x.SecurityQuestionIdFk == questionId || x.SecurityQuestionAnswer == answer);
                   
                if (result != null) { return true; }
                else { return false; }
            }
            else return false;
        }
        public void ChangeToNawPassword(int sellerId, string newPassword)
        {
            var res = _customer.Where(x=>x.Id == sellerId ).FirstOrDefault();
            res.Password = newPassword;
            _context.SaveChanges();
        }
    }
}
