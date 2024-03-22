namespace JobDone.Models.Banners
{
    public interface IBanner
    {
        public Task<IEnumerable<BannerModel>> getAllBanners();
    }
}
