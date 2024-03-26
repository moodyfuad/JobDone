using JobDone.Data;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.SellerOldWork
{
    public class SellerOldWorksImp : ISellerOldWork
    {
        private readonly JobDoneContext _db;
        private readonly DbSet<SellerOldWorkModel> _posts;

        public SellerOldWorksImp(JobDoneContext db)
        {
            _db = db;
            _posts = db.SellerOldWorkModels;
        }

        public async Task<SellerOldWorkModel> DeletePost(int postId)
        {
            SellerOldWorkModel? post = await _posts.FirstOrDefaultAsync(p=>p.Id==postId);
            _posts.Remove(post);
            _db.SaveChanges();
            return post;
        }

        public async Task<List<SellerOldWorkModel>> GetSellerOldWork(int sellerId)
        {
            return _db.SellerOldWorkModels.Where(s => s.SellerIdFk == sellerId).ToList();
        }
    }
}
