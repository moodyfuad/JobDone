using JobDone.Models;

namespace JobDone.ViewModels
{
    public class Admin_SellerServicesViewModel
    {
        public Admin_SellerServicesViewModel()
        {

        }

        public List<SellerModel> Sellers { get; set; }

        public struct Operations
        {
            public const string DeleteAccount = "Delete Account";
            public const string DeletePost = "Delete Post";
            public const string Transfer = "Transfer";
            public const string Withdraw = "Withdraw";
            public const string Deposit = "Deposit";
        }

        public decimal Amount { get; set; }

        public string CustomerUsername{ get; set; }
    }
}
