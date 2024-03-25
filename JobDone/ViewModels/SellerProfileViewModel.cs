using JobDone.Models;
using JobDone.Models.SellerOldWork;

namespace JobDone.ViewModels
{
    public class SellerProfileViewModel
    {
        public SellerModel sellerModels {  get; set; }
        public List<SellerOldWorkModel>? sellerOldWorkModels { get; set; }
    }
}
