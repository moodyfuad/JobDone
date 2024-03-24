using Humanizer;
using JobDone.Data;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.SellerAcceptRequest;
using JobDone.Models.Service;
using JobDone.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobDone.Models.Seller
{
    public class SellerImplemntation : ISeller
    {
        private readonly DbSet<SellerModel> _seller;
        private readonly DbSet<OrderModel> _order;
        private readonly DbSet<OrderByCustomerModel> _orderByCustomers;
        private readonly DbSet<SellerAcceptRequestModel> _sellerAcceptRequest; 
        private readonly DbSet<CustomerModel> _customer;
        private readonly JobDoneContext _Db;
        
        public SellerImplemntation(JobDoneContext context)
        {
            _seller = context.SellerModels;
            _order = context.OrderModels;
            _orderByCustomers = context.OrderByCustomerModels;
            _sellerAcceptRequest = context.SellerAcceptRequestModels;
            _customer = context.CustomerModels;
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

        public async Task<IEnumerable<SellerModel>> getAllSelersBasedOnUsername(string search)
        {
            var sellers = await getAllTheSeller();
            var filteredSellers = sellers
                .Where(s => s.Username.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return filteredSellers;
        }

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

        public async Task<IEnumerable<SellerModel>> GetAllSellersWithCategory(string search)
        {
            var sellers = await getAllTheSeller();
            var filteredSellers = sellers.Where(s => s.CategoryIdFkNavigation.Name.StartsWith(search, StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredSellers;
        }

        public async Task<IEnumerable<ServiceModel>> GetAllSellersWithService(string search)
        {
            var sellersWithService = _Db.ServiceModels
                .Join(_seller, s => s.Id, ss => ss.Id, (s, ss) => s)
                .Where(s => s.Name.StartsWith(search))
                .ToList();

            return sellersWithService;
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

        //public List<int> custemrname(int sellerId)
        //{
        //    var x = orderModels(sellerId).include().CustomerIdFk;
        //   return x;
        //}

        public List<CustomerModel> GetCustomerusername(int sellerId)
        {
            var result = _order
                .Where(o => o.SellerIdFk == sellerId)
                .Select(o => o.CustomerIdFkNavigation)
                .ToList();

            return result;
        }
        public List<OrderByCustomerModel> GetOrderByCustomerModels(int sellerCatgoreId)
        {
            var result = _orderByCustomers.Where(x=>x.CategoryIdKf == sellerCatgoreId).ToList();
            return result;
        }
        public int SellerCatgoreID(int sellerId)
        {
            return (int)_seller.Where(x => x.Id == sellerId).Select(x=>x.CategoryIdFk).FirstOrDefault();
        }
        public List<CustomerModel> CustomerReqwestWork(int sellerID)
        {
            return _orderByCustomers.Where(x=>x.CategoryIdKf==SellerCatgoreID(sellerID)).Select(x=>x.CustomerIdFkNavigation).ToList();
        }
        public List<SellerAcceptRequestModel> GetSellerAcceptRequestModels()
        {
            return _sellerAcceptRequest.ToList();
        }
        public void SaveSellerAccept(SellerAcceptRequestModel SAR)
        {
            _sellerAcceptRequest.Add(SAR);
            _Db.SaveChanges();
        }

        public List<SellerModel> GetSellersWhoAcceptedRequest(List<int> sellersId)
        {
            List<SellerModel> sellers = new List<SellerModel>();
            foreach (int id in sellersId)
            {
                sellers.Add(GetSellerById(id));
            }
            return sellers;
        }
    }
}
