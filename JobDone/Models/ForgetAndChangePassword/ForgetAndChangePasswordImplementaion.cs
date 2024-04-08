using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.ForgetAndChangePassword
{
    public class ForgetAndChangePasswordImplementaion : IForgetAndChanePassword
    {
        private readonly JobDoneContext _context;
        private readonly DbSet<SellerModel> _seller;
        private readonly DbSet<CustomerModel> _customer;
        private readonly DbSet<SecurityQuestionModel> _questions;

        public ForgetAndChangePasswordImplementaion(JobDoneContext context)
        {
            _context = context;
            _seller = context.SellerModels;
            _customer = context.CustomerModels;
            _questions = context.SecurityQuestionModels;
        }

        public int ConfirmTheAnswerForTheCustomer(string username, int questionId, string answer)
        {
            if (answer != null && username != null && questionId != 0)
            {
                var result = _customer
                   .FirstOrDefault(x => x.Username == username && x.SecurityQuestionIdFk == questionId && x.SecurityQuestionAnswer == answer);
                   
                if (result != null) { return result.Id; }
                else { return 0; }
            }
            else return 0;
        }
        public void ChangeToNawPassword(int sellerId, string newPassword)
        {
            var res = _customer.Where(x=>x.Id == sellerId ).FirstOrDefault();
            res.Password = newPassword;
            _context.SaveChanges();
        }
        public int ConfirmTheAnswerForTheSeller(string username, int questionId, string answer)
        {
            if (answer != null && username != null && questionId != 0)
            {
                var result = _seller
                   .FirstOrDefault(x => x.Username == username && x.SecurityQuestionIdFk == questionId && x.SecurityQuestionAnswer == answer);

                if (result != null) { return result.Id; }
                else { return 0; }
            }
            else return 0;
        }
        public void ChangeToNawPasswordSeller(int sellerId, string newPassword)
        {
            var res = _seller.Where(x => x.Id == sellerId).FirstOrDefault();
            res.Password = newPassword;
            _context.SaveChanges();
        }
    }
}
