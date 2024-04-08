using JobDone.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;

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

        public void ConvertStatusToDone(WithdrawModel withdraw)
        {
            withdraw.Status = 1;
            _withdrawModels.Update(withdraw);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<WithdrawModel>> GetAllWithdrawRequest()
        {
            return await _withdrawModels.Include("SellerIdFkNavigation").Where(w => w.Status == 0).ToListAsync();
        }

        public async Task<WithdrawModel> GetWithdrawInfoById(short Id)
        {
            return _withdrawModels.Include("SellerIdFkNavigation").FirstOrDefault(w => w.Id == Id);
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
