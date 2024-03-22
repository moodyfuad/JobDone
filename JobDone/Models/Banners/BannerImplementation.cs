using JobDone.Data;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Banners
{
    public class BannerImplementation : IBanner
    {
        private readonly JobDoneContext _context;

        public BannerImplementation(JobDoneContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BannerModel>> getAllBanners()
        {
            return await _context.BannerModels.Where(forWho => forWho.ForWho == 1).ToListAsync();
        }
    }
}
