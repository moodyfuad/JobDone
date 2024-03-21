using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobDone.Models.Seller
{
    public class SellerImplemntation : ISeller
    {
        private readonly DbSet<SellerModel> _seller;
        private readonly JobDoneContext _Db;
        
        public SellerImplemntation(JobDoneContext context)
        {
            _seller = context.SellerModels;
            _Db = context;
        }

        void SaveSellerInDB(SellerModel seller)
        {
            _seller.Add(seller);
            _Db.SaveChanges();
        }

        public bool UsernameExist(SellerModel seller)
        {
            var sel = _seller.Where(c => c.Username == seller.Username);
            if (sel == null)
            {
                return true;
            }
            else { return false; }

        }

        public byte[] ConvertToByte(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.CopyToAsync(memoryStream).Wait();

                return memoryStream.ToArray(); 
            }
        }

        public void SignUp(SellerModel seller)
        {
            SaveSellerInDB(seller);
        }

        public bool CheckUsernameAndPasswordExists(SellerModel seller)
        {
            var sel = _seller.FirstOrDefault(c => c.Username == seller.Username && c.Password == seller.Password);

            if (sel != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public SellerModel GetSellerById(int id)
        {
            return _seller.FirstOrDefault(s => s.Id == id);
        }


        public IFormFile ConvertToImage(byte[] byteImage)
        {
            using(var memoryStream = new MemoryStream(byteImage))
            {
                memoryStream.Position = 0;
                FormFile image = new FormFile(memoryStream, 0, memoryStream.Length, "picture.jpj", "image/jpeg");
                return image;
            }
        }
    }
}
