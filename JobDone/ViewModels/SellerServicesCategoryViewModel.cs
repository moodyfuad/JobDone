using JobDone.Models.Category;
using JobDone.Models;
using JobDone.Models.SellerOldWork;

namespace JobDone.ViewModels
{
    public class SellerServicesCategoryViewModel
    {
        public SellerModel Seller { get; set; }
        public IEnumerable<ServiceModel> Services { get; set; }
        public IEnumerable<SellerOldWorkModel> sellerOldWorks { get; set; }
        public short customerId { get; set; }
    }
}
