namespace JobDone.Models.Banners
{
    public interface IBanner
    {
        public Task<IEnumerable<BannerModel>> GetAllCustomerBanners();
        public Task<IEnumerable<BannerModel>> GetAllSellerBanners();
        public Task<IEnumerable<BannerModel>> GetAllAdminBanners();

        public void ModeifyBanner(BannerModel banner, IFormFile picFile);
        public BannerModel GetBannerById(int id);
        public void AddNewBannerInCustomer(IFormFile banner);
        public void AddNewBannerInSeller(IFormFile banner);
        public void Delete(BannerModel banner);
    }
}
