namespace JobDone.Models.Banners
{
    public interface IBanner
    {
        public Task<IEnumerable<BannerModel>> GetAllCustomerBanners();
        public Task<IEnumerable<BannerModel>> GetAllSellerBanners();
        public Task<IEnumerable<BannerModel>> GetAllAdminBanners();

        public Task ModeifyBanner(BannerModel banner, IFormFile picFile);
        public BannerModel GetBannerById(int id);
        public Task AddNewBannerInCustomer(IFormFile banner);
        public Task AddNewBannerInSeller(IFormFile banner);
        public Task Delete(BannerModel banner);
    }
}
