using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobDone.Models.Seller
{
    public class SellerImplemntation : ISeller
    {
        private readonly DbSet<SellerModel> _seller;
        private readonly DbSet<OrderModel> _order;
        private readonly DbSet<CustomerModel> _customer;
        private readonly JobDoneContext _Db;
        
        public SellerImplemntation(JobDoneContext context)
        {
            _seller = context.SellerModels;
            _order = context.OrderModels;
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

        public short getId(string username, string password)
        {
            var sl = _seller.FirstOrDefault(info => info.Username == username && info.Password == password);
            return (short)sl.Id;
        }

        public decimal GetWallet(int id)
        {
            return Convert.ToDecimal(_Db.SellerModels.SingleOrDefault(x => x.Wallet == id));
        }

        //public decimal GetTodaySale(int id)
        //{
        //   var todayMoney = _Db.SellerModels.SingleOrDefault(x => x.Id==id);
        //    todayMoney.Wallet

        //}

        public int GetRemainingWork(int id)
        {
            var remainingWork = _Db.OrderModels.Count(x => x.SellerIdFk == id);
            return remainingWork;
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

        public int AveilabelRReqest(int sellerId)
        {
            SellerModel seller = _seller.FirstOrDefault(s => s.Id==sellerId);
            int categoryfk = seller.CategoryIdFk; 
           return _Db.OrderByCustomerModels.Where(x => x.CategoryIdKf == categoryfk).Count();
        }

        public Decimal Totalgains(int sellerId)
        {
            return _order.Where(x => x.SellerIdFk == sellerId).Sum(x => x.Price) ;
        }

        public async Task<IEnumerable<SellerModel>> getAllTheSeller()
        {
            return await _seller.Include("CategoryIdFkNavigation").ToListAsync();
        }

        public int OrderCount(int sellerId)
        {
            return _order.Where(x=>x.SellerIdFk==sellerId).Count();
        }
        public string OrderName(int sellerId)
        {
            //OrderModel order = _order.FirstOrDefault(x=>x.SellerIdFk==sellerId);
            //string orderName = order.OrderName;
            return "orderName";
        }

        public List<OrderModel> orderModels(int sellerId)
        {
           List<OrderModel> order = _order.Where(s => s.SellerIdFk == sellerId).ToList();
            return order;
        }
        
        public int customerID( int ordrId)
        {
            var cuId = _order.Where(x => x.Id == ordrId).Select(x=>x.CustomerIdFk).ToList();
            return cuId[0];
        }
        //public List<CustomerModel> GetCustomerName(int CustomerId)
        //{
            
        //        var cuName = _customer.Where(x => x.Id == ).ToList();  
           
        //    //foreach (var c in cuID)
        //    //{
        //    //    return cuNAme[0].FirstName + " " + cuNAme[0].LastName;
        //    //}
        //    return cuName; 
        //}
    }
}
