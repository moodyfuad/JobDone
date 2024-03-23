using JobDone.Models;
using JobDone.Models.Category;

namespace JobDone.ViewModels
{
    public class SellerServicesCategoryViewModel
    {
        public IEnumerable<SellerModel>? Sellers { get; set; }
        public IEnumerable<ServiceModel>? Services { get; set; }
        public IEnumerable<CategoryModel>? Categories { get; set; }
        public string Option { get; set; }
    }
}
