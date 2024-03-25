using JobDone.Data;

namespace JobDone.Models.SellerOldWork
{
    public class SellerOldWorksImp : ISellerOldWork
    {
        private readonly JobDoneContext _db;

        public SellerOldWorksImp(JobDoneContext db)
        {
            _db = db;
        }

        public async Task<List<SellerOldWorkModel>> GetSellerOldWork(int sellerId)
        {
            return _db.SellerOldWorkModels.Where(s => s.SellerIdFk == sellerId).ToList();
        }
    }
}
