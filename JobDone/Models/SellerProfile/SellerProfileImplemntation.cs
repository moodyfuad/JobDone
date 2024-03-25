using JobDone.Data;
using JobDone.Models.SellerOldWork;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.SellerProfile
{
    public class SellerProfileImplemntation : ISellerProfile
    {
        private readonly DbSet<SellerModel> _seller;
        private readonly DbSet<SellerOldWorkModel> _sellerOldWork;

        public SellerProfileImplemntation(JobDoneContext context)
        {
            _seller = context.SellerModels;
            _sellerOldWork = context.SellerOldWorkModels;
        }
        public List<SellerModel> GetSellerProfile(int sellerID)
        {
            return _seller.Where(x=>x.Id == sellerID).ToList();
        }

    }
}
