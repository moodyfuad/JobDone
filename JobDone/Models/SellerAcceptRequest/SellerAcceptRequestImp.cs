
using JobDone.Data;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NuGet.Packaging;
using SQLitePCL;

namespace JobDone.Models.SellerAcceptRequest
{
    public class SellerAcceptRequestImp : ISellerAcceptRequest
    {
        private readonly JobDoneContext _db;
        private readonly DbSet<SellerAcceptRequestModel> _sellersWhoAccept;

        public SellerAcceptRequestImp(JobDoneContext db)
        {
            _db = db;
            _sellersWhoAccept = _db.SellerAcceptRequestModels;

        }

        public async Task<List<SellerModel>?>GetSellersId(int orderId)
        {
            List<SellerModel>? sellers = new();
            List<SellerAcceptRequestModel> recordes =
                await _sellersWhoAccept.Where(s => s.OrderByCustomerIdFk == orderId).ToListAsync();
            foreach (SellerAcceptRequestModel record in recordes)
            {
                SellerModel seller = await _db.SellerModels.FirstOrDefaultAsync(s => s.Id == record.SellerIdFk);
                ICollection<ServiceModel> services = await _db.ServiceModels.
                    Where(ser => seller.Id == ser.SellerIdFk).ToListAsync();
/*                seller.ServiceModels.AddRange(services);
*/                sellers.Add(seller);
            }
            return sellers;
        }

        public async Task<SellerAcceptRequestModel> RemoveSeller(int sellerId, int OrderByCustomerId)
        {
            SellerAcceptRequestModel request = _db.SellerAcceptRequestModels.FirstOrDefault(
                SAM => SAM.SellerIdFk == sellerId && SAM.OrderByCustomerIdFk == OrderByCustomerId);
            _db.SellerAcceptRequestModels.Remove(request);
            _db.SaveChanges();

            return request;
        }
        
        public async Task<bool> AcceptSeller(int sellerId, int OrderByCustomerId)
        {
            List<SellerAcceptRequestModel> requests = _db.SellerAcceptRequestModels.Where(
                SAM => SAM.SellerIdFk != sellerId && SAM.OrderByCustomerIdFk == OrderByCustomerId).ToList();

                OrderByCustomerModel orderByCustomer = _db.OrderByCustomerModels.FirstOrDefault(o => o.Id == OrderByCustomerId);

            CustomerModel customer = _db.CustomerModels.FirstOrDefault(c => c.Id == orderByCustomer.CustomerIdFk);
            if (customer.Wallet >= orderByCustomer.Price)
            {
                 customer.Wallet -= orderByCustomer.Price;
                 _db.CustomerModels.Update(customer);
                try
                {
                
                    SellerAcceptRequestModel request = RemoveSeller(sellerId, OrderByCustomerId).Result;



                    CategoryModel category = _db.CategoryModels.FirstOrDefault(c => c.Id == orderByCustomer.CategoryIdKf);


                OrderModel orderModel = new OrderModel()
                {
                    SellerIdFk = sellerId,
                    CustomerIdFk = orderByCustomer.CustomerIdFk,
                    CategoryIdKf = category.Id,
                    DeliverDate = orderByCustomer.DeliverDate,
                    Description = orderByCustomer.Description,
                    OrderDate = orderByCustomer.OrderDate,
                    OrderName = orderByCustomer.OrderName,
                    Price = orderByCustomer.Price,
                    Status = "working",
                    CategoryIdKfNavigation = category,
                };

                _db.OrderModels.Add(orderModel);

                    _db.SellerAcceptRequestModels.RemoveRange(requests);
                    _db.OrderByCustomerModels.Remove(orderByCustomer);
                    _db.SaveChanges();
                }
                catch (Exception ex) { }
                return true;
            }
            return false;

        }
    }
}
