namespace JobDone.Models.Banners
{
    public interface IBanner
    {
        public Task<IEnumerable<BannerModel>> GetAllCustomerBanners();
        public Task<IEnumerable<BannerModel>> GetAllSellerBanners();
        public Task<IEnumerable<BannerModel>> GetAllAdminBanners();

        public void ModeifyBanner(BannerModel banner, IFormFile picFile);
        public BannerModel GetBannerById(int id);
    }
}
