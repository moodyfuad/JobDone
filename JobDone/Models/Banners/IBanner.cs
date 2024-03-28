namespace JobDone.Models.Banners
{
    public interface IBanner
    {
        public Task<IEnumerable<BannerModel>> GetAllCustomerBanners();
        public Task<IEnumerable<BannerModel>> GetAllSellerBanners();
    }
}
