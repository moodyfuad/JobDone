using JobDone.Data;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace JobDone.Models.Banners
{
    public class BannerImplementation : IBanner
    {
        private readonly JobDoneContext _context;
        private readonly DbSet<BannerModel> _banner;

        public BannerImplementation(JobDoneContext context)
        {
            _context = context;
            _banner = _context.BannerModels;
        }

        public async Task<IEnumerable<BannerModel>> GetAllAdminBanners()
        {
            return await _banner.Where(forWho => forWho.ForWho == 3).ToListAsync();
        }

        public async Task<IEnumerable<BannerModel>> GetAllCustomerBanners()
        {
            return await _banner.Where(forWho => forWho.ForWho == 1).ToListAsync();
        }
        
        public async Task<IEnumerable<BannerModel>> GetAllSellerBanners()
        {
            return await _banner.Where(forWho => forWho.ForWho == 2).ToListAsync();
        }

        public BannerModel GetBannerById(int id)
        {
            return _banner.FirstOrDefault(x => x.Id == id);
        }

        public async Task Delete(BannerModel banner)
        {
            _banner.Remove(banner);
            _context.SaveChangesAsync();
        }

        public async Task AddNewBannerInCustomer(IFormFile banner)
        {
            if (banner != null)
            {
                byte[] pictureBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await banner.CopyToAsync(memoryStream);
                    pictureBytes = memoryStream.ToArray();
                }

                BannerModel bannerModel = new BannerModel()
                {
                    Picture = pictureBytes,
                    ForWho = 1,
                };

                _banner.Add(bannerModel);
                _context.SaveChangesAsync().Wait();
            }
        }

        public async Task AddNewBannerInSeller(IFormFile banner)
        {
            byte[] pictureBytes;
            if (banner != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await banner.CopyToAsync(memoryStream);
                    pictureBytes = memoryStream.ToArray();
                }

                BannerModel bannerModel = new BannerModel()
                {
                    Picture = pictureBytes,
                    ForWho = 2,
                };

                _banner.Add(bannerModel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ModeifyBanner(BannerModel banner, IFormFile picFile)
        {
            if (banner != null && picFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await picFile.CopyToAsync(memoryStream);
                    banner.Picture = memoryStream.ToArray();
                }

                _context.Update(banner);
                _context.SaveChangesAsync().Wait();
            }
        }
    }
}
