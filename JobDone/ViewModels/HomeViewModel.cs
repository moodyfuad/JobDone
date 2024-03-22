using JobDone.Models;
using JobDone.Models.Banners;

namespace JobDone.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<SellerModel>? Sellers{ get; set; }
        public IEnumerable<ServiceModel>? Services { get; set; }
        public IEnumerable<BannerModel>? Bans { get; set; }
    }
}
