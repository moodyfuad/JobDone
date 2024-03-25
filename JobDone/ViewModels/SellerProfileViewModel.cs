using JobDone.Models;

namespace JobDone.ViewModels
{
    public class SellerProfileViewModel
    {
        public SellerModel sellerModels {  get; set; }
        public List<SellerOldWorkModel>? sellerOldWorkModels { get; set; }
    }
}
