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

        public void Delete(BannerModel banner)
        {
            _banner.Remove(banner);
            _context.SaveChanges();
        }

        public void AddNewBannerInCustomer(IFormFile banner)
        {
            if(banner != null)
            {
                byte[] pictureBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    banner.CopyTo(memoryStream);
                    pictureBytes = memoryStream.ToArray();
                }

                BannerModel bannerModel = new BannerModel()
                {
                    Picture = pictureBytes,
                    ForWho = 1,
                };

                _banner.Add(bannerModel);
                _context.SaveChanges();
            }
        }

        public void AddNewBannerInSeller(IFormFile banner)
        {
            byte[] pictureBytes;
            if(banner != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    banner.CopyTo(memoryStream);
                    pictureBytes = memoryStream.ToArray();
                }

                BannerModel bannerModel = new BannerModel()
                {
                    Picture = pictureBytes,
                    ForWho = 2,
                };

                _banner.Add(bannerModel);
                _context.SaveChanges();
            }
        }

        public void ModeifyBanner(BannerModel banner, IFormFile picFile)
        {
            if (banner != null && picFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    picFile.CopyTo(memoryStream);
                    banner.Picture = memoryStream.ToArray();
                }

                _context.Update(banner);
                _context.SaveChanges();
            }
        }
    }
}
