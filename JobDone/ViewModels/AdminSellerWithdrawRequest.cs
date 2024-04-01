using JobDone.Models;
using JobDone.Models.Withdraw;

namespace JobDone.ViewModels
{
    public class AdminSellerWithdrawRequest
    {
        public IEnumerable<WithdrawModel> Withdraws { get; set; }
        public int TotalRequest { get; set; }
        public int TotalRequestToShow { get; set; }
        public string Option { get; set; }

        public int Count { get; set; }
    }
}
