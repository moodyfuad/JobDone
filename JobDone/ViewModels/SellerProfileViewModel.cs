using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.SellerOldWork;
using JobDone.Models.Withdraw;

namespace JobDone.ViewModels
{
    public class SellerProfileViewModel
    {
        public SellerModel sellerModels {  get; set; }
        public List<SellerOldWorkModel>? sellerOldWorkModels { get; set; }
        public List<CategoryModel>? Category { get; set; }
        public SellerOldWorkModel OneSellerOldWorkModel { get; set; }
        public List<ServiceModel>? serviceModels { get; set; }
        public ServiceModel service { get; set; }
        public WithdrawModel? withdrawModels { get; set; }
    }
}
