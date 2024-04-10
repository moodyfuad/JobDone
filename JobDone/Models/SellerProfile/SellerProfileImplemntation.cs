using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.Models.SellerOldWork;
using Microsoft.EntityFrameworkCore;
using JobDone.Models.Service;
using JobDone.Models.Category;
using JobDone.Models.Withdraw;
using JobDone.Models.Seller;

namespace JobDone.Models.SellerProfile
{
    public class SellerProfileImplemntation : ISellerProfile
    {

        private readonly DbSet<SellerModel> _seller;
        private readonly DbSet<SellerOldWorkModel> _sellerOldWork;
        private readonly DbSet<ServiceModel> _service;
        private readonly DbSet<WithdrawModel> _withdrawModels;
        private readonly DbSet<CategoryModel> _category;
        private readonly JobDoneContext _Db;
        private readonly byte[] DefualtImage = new byte[]
{
    0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x01, 0x01, 0x00, 0x48,
    0x00, 0x48, 0x00, 0x00, 0xFF, 0xDB, 0x00, 0x43, 0x00, 0x08, 0x06, 0x06, 0x07, 0x06, 0x05, 0x08, 0x07,
    0x07, 0x07, 0x09, 0x09, 0x08, 0x0A, 0x0C, 0x14, 0x0D, 0x0C, 0x0B, 0x0B, 0x0C, 0x19, 0x12, 0x13, 0x0F,
    0x14, 0x1D, 0x1A, 0x1F, 0x1E, 0x1D, 0x1A, 0x1C, 0x1C, 0x20, 0x24, 0x2E, 0x27, 0x20, 0x22, 0x2C, 0x23,
    0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C,
    0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C,
    0xFF, 0xC4, 0x00, 0x1F, 0x01, 0x00, 0x03, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B,
    0xFE, 0x00, 0x3B, 0x43, 0x52, 0x45, 0x41, 0x54, 0x4F, 0x52, 0x3A, 0x20, 0x20, 0x4F, 0x70, 0x65, 0x6E,
    0x41, 0x49, 0x20, 0x47, 0x65, 0x6E, 0x65, 0x72, 0x61, 0x74, 0x65, 0x64, 0x20, 0x42, 0x79, 0x20, 0x4F,
    0x70, 0x65, 0x6E, 0x41, 0x49, 0x2E, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

        public SellerProfileImplemntation(JobDoneContext context)
        {
            _seller = context.SellerModels;
            _category = context.CategoryModels;
            _sellerOldWork = context.SellerOldWorkModels;
            _service = context.ServiceModels;
            _withdrawModels = context.WithdrawModels;
            _Db = context;
        }
        public SellerModel GetSellerProfile(int sellerID)
        {
            return _seller.FirstOrDefault(x=>x.Id == sellerID);
        }
        public List<CategoryModel> GetCategories()
        {
            return _category.ToList();
        }
        public List<ServiceModel> GetServiceModels(int sellerID)
        {
            return _service.Include("SellerIdFkNavigation")
                    .Where(x => x.SellerIdFk == sellerID)
                    .ToList();

        }
        public bool IsWithdrawAmountbefore(int sellerID)
        {
            var result = _withdrawModels.Where(x => x.SellerIdFk == sellerID && x.Status == 0).FirstOrDefault();
            if (result == null)
                return false;
            else return true;
        }
        public void AddWithdrawMoney(WithdrawModel AWDM)
        {
            _withdrawModels.Add(AWDM);
            _Db.SaveChanges();
        }
        public List<WithdrawModel> GetAllwithdrawForOneSeller(int sellerId)
        {
            return _withdrawModels.Where(x=>x.SellerIdFk == sellerId).ToList();
        }
        public bool UsernameExist(string username)
        {
            var sl = _seller.Where(c => c.Username == username);
            if (sl.Count() != 0)
            {
                return true;
            }
            else { return false; }
        }
        public byte[] ConvertToByteArray(IFormFile image)
        {
            if (image == null)
            {
                return DefualtImage;
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyToAsync(memoryStream).Wait();
                    return (memoryStream.ToArray());
                }
            }

        }
        public void ApplyChangesToSeller(ref SellerModel Seller, SellerModel ViewModelSeller)
        {
            if (Seller.Username != ViewModelSeller.Username)
            {
                Seller.Username = ViewModelSeller.Username;
            }

            if (Seller.FirstName != ViewModelSeller.FirstName)
            {
                Seller.FirstName = ViewModelSeller.FirstName;
            }

            if (Seller.LastName != ViewModelSeller.LastName)
            {
                Seller.LastName = ViewModelSeller.LastName;
            }

            if (Seller.Email != ViewModelSeller.Email)
            {
                Seller.Email = ViewModelSeller.Email;
            }

            if (Seller.PhoneNumber != ViewModelSeller.PhoneNumber)
            {
                Seller.PhoneNumber = ViewModelSeller.PhoneNumber;
            }

            if (Seller.BirthDate != ViewModelSeller.BirthDate)
            {
                Seller.BirthDate = ViewModelSeller.BirthDate;
            }

        }

        public ServiceModel GetServiceInfo(ServiceModel service)
        {
            return _service.FirstOrDefault(x => x.SellerIdFk == service.SellerIdFk && x.Name == service.Name);
        }

        public List<SellerOldWorkModel> GetSellerOldWorkModels(int sellerID) 
        {
            return _sellerOldWork.Where(x=>x.SellerIdFk == sellerID).ToList();
        }
        public void DeleteOldWork(int oldWorkId)
        {
            SellerOldWorkModel result = _sellerOldWork.Where(x => x.Id == oldWorkId).FirstOrDefault();
            _sellerOldWork.Remove(result);
            _Db.SaveChanges();
        }
        public void editOldWork(int oldworkId, IFormFile newphoto, string newdescrepion)
        {
            var editwork = _sellerOldWork.Where(x => x.Id == oldworkId).FirstOrDefault();
            editwork.Picture = ConvertToByteArray(newphoto);
            editwork.Description = newdescrepion;
            _Db.SaveChanges();
        }
        public SellerOldWorkModel GetOneSellerOldWorkModel(int oldWorkId) 
        {
            return _sellerOldWork.Where(x=>x.Id == oldWorkId).FirstOrDefault();
        }
        public void AddNewWork(IFormFile imge, string description, int sellerId)
        {
            SellerOldWorkModel nweWork = new SellerOldWorkModel
            {
                Picture = ConvertToByteArray(imge),
                Description = description,
                SellerIdFk = sellerId
            };
            _sellerOldWork.Add(nweWork);
            _Db.SaveChanges();
        }
    }
}
