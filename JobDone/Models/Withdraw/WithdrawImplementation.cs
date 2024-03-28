using JobDone.Data;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Withdraw
{
    public class WithdrawImplementation : IWithdraw
    {
        private readonly DbSet<WithdrawModel> _withdrawModels;
        private readonly JobDoneContext _context;

        public WithdrawImplementation(JobDoneContext context)
        {
            _context = context;
            _withdrawModels = _context.WithdrawModels;
        }

        public void SendRequestToPullMoneyToAdmin(short customerId, int amount)
        {
            WithdrawModel withdrawModel = new WithdrawModel() 
            {
                AmountOfMoney = amount,
            };
        }
    }
}
