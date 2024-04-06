using Humanizer;
using JobDone.Data;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.SellerAcceptRequest;
using JobDone.Models.SellerOldWork;
using JobDone.Models.Service;
using JobDone.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        private readonly DbSet<SellerOldWorkModel> _posts;

        public SellerImplemntation(JobDoneContext context)
        {
            _seller = context.SellerModels;
            _order = context.OrderModels;
            _orderByCustomers = context.OrderByCustomerModels;
            _sellerAcceptRequest = context.SellerAcceptRequestModels;
            _customer = context.CustomerModels;
            _Db = context;
            _posts = context.SellerOldWorkModels;
        }

        void SaveSellerInDB(SellerModel seller)
        {
            _seller.Add(seller);
            _Db.SaveChanges();
        }

        public bool UsernameExist(string username)
        {
            var sel = _seller.Where(s => s.Username == username);
            if (sel != null)
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
                return true;
            else
                return false;
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

        public List<OrderByCustomerModel> getAllOrderByCustomerBasedOnOrdername(string search, int sellerId)
        {
            var orders = GetOrderByCustomerModels(SellerCatgoreID(sellerId), sellerId);
            var filteredSellers = orders
                .Where(o => o.OrderName.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return filteredSellers;
        }

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

        public int GetSARMForOneSeller(int sellerId)
        {
            return _sellerAcceptRequest.Where(x => x.SellerIdFk == sellerId).Count();
        }

        public SellerModel GetSellerById(int id)
        {
            return _seller.Include("CategoryIdFkNavigation").FirstOrDefault(s => s.Id == id);
        }

        public IFormFile ConvertToImage(byte[] byteImage)
        {
            using (var memoryStream = new MemoryStream(byteImage))
            {
                memoryStream.Position = 0;
                FormFile image = new FormFile(memoryStream, 0, memoryStream.Length, "picture.jpj", "image/jpeg");
                return image;
            }
        }

        public int AveilabelRReqest(int sellerId)
        {
            SellerModel seller = _seller.FirstOrDefault(s => s.Id == sellerId);
            int categoryfk = seller.CategoryIdFk;
            return _Db.OrderByCustomerModels.Where(x => x.CategoryIdKf == categoryfk).Count();
        }

        public Decimal Totalgains(int sellerId)
        {
            return _order.Where(x => x.SellerIdFk == sellerId).Sum(x => x.Price);
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
            return _order.Where(x => x.SellerIdFk == sellerId).Count();
        }

        public List<OrderModel> orderModels(int sellerId)
        {
            List<OrderModel> order = _order
                .Include(o => o.CategoryIdKfNavigation)
                .Include(o => o.CustomerIdFkNavigation)
                .Include(o => o.SellerIdFkNavigation)
                .Where(s => s.SellerIdFk == sellerId)
                .ToList();

            return order;
        }

        public void ChangeOrderStatus(int orderID)
        {
            var status = _order.Where(x => x.Id == orderID).FirstOrDefault();
            status.Status = "SellerCompleted";
            _Db.SaveChanges();
        }

        public void DeleteOrder(int orderID)
        {
            var result = _order.Where(x => x.Id == orderID).FirstOrDefault();
            _order.Remove(result);
            _Db.SaveChanges();
        }

        public List<CustomerModel> GetCustomerusername()
        {
            var result = _customer.ToList();

            return result;
        }

        public List<OrderByCustomerModel> GetOrderByCustomerModels(int sellerCatgoreId, int sellerId)
        {
            var result = _orderByCustomers.Where(x => x.CategoryIdKf == sellerCatgoreId).ToList();
            return result;
        }

        public async Task<List<int>> GetRequestsThatTheSellerAccept(int sellerId)
        {
            var res = _sellerAcceptRequest.Where(x => x.SellerIdFk == sellerId).Select(x=>x.OrderByCustomerIdFk).ToListAsync();
            return await res;
        }

        public List<OrderByCustomerModel> GetOrderByCustomerModelsFiveCustomer(int sellerCatgoreId)
        {
            var result = _orderByCustomers.Where(x => x.CategoryIdKf == sellerCatgoreId).Take(5).ToList();
            return result;
        }

        public int SellerCatgoreID(int sellerId)
        {
            return (int)_seller.Where(x => x.Id == sellerId).Select(x => x.CategoryIdFk).FirstOrDefault();
        }

        public List<CustomerModel> CustomerReqwestWork(int sellerID)
        {
            return _orderByCustomers.Where(x => x.CategoryIdKf == SellerCatgoreID(sellerID)).Select(x => x.CustomerIdFkNavigation).ToList();
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

        public List<SellerModel> GetFirst10()
        {
            return _Db.SellerModels.Take(10).ToList();
        }

        public List<SellerModel> GetFirst10(string username)
        {
            List<SellerModel> sellers = _Db.SellerModels.Where(s =>
                s.Username.ToLower().StartsWith(username.ToLower()) || s.Username.ToLower().EndsWith(username.ToLower())).ToList();
            if (sellers.Count == 0)
            {
                return null;
            }
            else if (sellers.Count == 1)
            {
                sellers.First().SellerOldWorkModels = _posts.Where(p => p.SellerIdFk == sellers.First().Id).ToList();
                return sellers;
            }
            else if (sellers.Count < 10)
            {
                return sellers;
            }
            else
            {
                return sellers.Take(10).ToList();
            }
        }
        public async Task<bool> DeleteAccount(int sellerId)
        {
            try
            {
                SellerModel? seller = _seller.Include(s => s.ServiceModels).
                                              Include(s => s.SellerOldWorkModels).
                                              Include(s => s.OrderModels).
                                              Include(s => s.WithdrawModels).
                                              Include(s => s.SellerAcceptRequestModels).
                                              Include(s => s.SecurityQuestionIdFkNavigation).
                                              Include(s => s.CategoryIdFkNavigation).
                                              FirstOrDefaultAsync(s => s.Id == sellerId).Result;

                try
                {
                    _Db.SellerModels.Remove(seller);
                    await _Db.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch { return false; }

        }

        public async Task<List<SellerModel>>? GetSellerWithPosts(int sellerId)
        {
            List<SellerModel> sellers =
                _seller.Where(seller => seller.Id == sellerId).Include(s => s.SellerOldWorkModels).ToList();

            return sellers;
        }


        public async Task<SellerModel> Withdraw(SellerModel seller, decimal MoneyAmount)
        {
            seller.Wallet -= MoneyAmount;
            try
            {
                _seller.Update(seller);
                _Db.SaveChanges();

                return seller;
            }
            catch
            {
                return null;
            }

        }
        public async Task<SellerModel> Diposit(SellerModel seller, decimal MoneyAmount)
        {
            seller.Wallet += MoneyAmount;
            try
            {
                _seller.Update(seller);
                _Db.SaveChanges();

                return seller;
            }
            catch
            {
                return null;
            }
        }

    }
}
