
using JobDone.Data;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.Seller;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly DbSet<SellerModel> _sellers;
        private readonly DbSet<AdminWalletModel> _adminWallet;

        public SellerAcceptRequestImp(JobDoneContext db)
        {
            _db = db;
            _sellersWhoAccept = _db.SellerAcceptRequestModels;
            _sellers = _db.SellerModels;
            _adminWallet = _db.AdminWalletModels;

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

        public async Task<SellerAcceptRequestModel>? RemoveSeller(int sellerId, int OrderByCustomerId)
        {
            SellerAcceptRequestModel? request = _db.SellerAcceptRequestModels.FirstOrDefault(
                SAM => SAM.SellerIdFk == sellerId && SAM.OrderByCustomerIdFk == OrderByCustomerId);
            try
            {
                if (request != null)
                {
                    _db.SellerAcceptRequestModels.Remove(request);
                    _db.SaveChanges();
                    return request;
                }
                return null;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> AcceptSeller(int sellerId, int OrderByCustomerId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // Website taxes percentage
                    decimal taxPercentage = 5m / 100m;

                    // Get the sellers from specific request who accepted the order except the seller that the customer accepts
                    List<SellerAcceptRequestModel> requests = _db.SellerAcceptRequestModels
                        .Where(SAM => SAM.SellerIdFk != sellerId && SAM.OrderByCustomerIdFk == OrderByCustomerId)
                        .ToList();

                    OrderByCustomerModel? orderByCustomer = _db.OrderByCustomerModels.FirstOrDefault(o => o.Id == OrderByCustomerId);
                    CustomerModel? customer = _db.CustomerModels.FirstOrDefault(c => c.Id == orderByCustomer.CustomerIdFk);
                    SellerModel? seller = _sellers.FirstOrDefault(s => s.Id == sellerId);
                    AdminWalletModel adminWallet = await _adminWallet.FirstAsync();

                    if (customer.Wallet >= orderByCustomer.Price)
                    {
                        // Take the order price from the customer
                        customer.Wallet -= orderByCustomer.Price;

                        // Transfer the money of the order to the seller and take 5% for the website
                        seller.Wallet += (orderByCustomer.Price - (taxPercentage * orderByCustomer.Price));

                        // Transfer 5% of the order money to the admin wallet
                        adminWallet.Balance += (taxPercentage * orderByCustomer.Price);

                        _db.CustomerModels.Update(customer);
                        _adminWallet.Update(adminWallet);
                        _sellers.Update(seller);

                        // Remove the seller that the customer accepts from the request list
                        SellerAcceptRequestModel request = await RemoveSeller(sellerId, OrderByCustomerId);

                        // Get the category related to the request
                        CategoryModel? category = _db.CategoryModels.FirstOrDefault(c => c.Id == orderByCustomer.CategoryIdKf);

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
                            Status = "Working",
                            CategoryIdKfNavigation = category,
                        };

                        _db.OrderModels.Add(orderModel);
                        _db.SellerAcceptRequestModels.RemoveRange(requests);
                        _db.OrderByCustomerModels.Remove(orderByCustomer);
                        _db.SaveChanges();

                        transaction.Commit();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
