using JobDone.Data;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Order
{
    public class OrderImplementation : IOrder
    {
        private readonly JobDoneContext _db;
        private readonly DbSet<OrderModel> _orders;

        public OrderImplementation(JobDoneContext context)
        {
            _db = context;
            _orders = _db.OrderModels;
        }

        public int GetCustomerIdFromOrder(int orderByCustomerId)
        {
            return _orders.FirstOrDefault(x => x.Id == orderByCustomerId).CustomerIdFk;
        }
        
        public List<OrderModel>? GetCustomerOrders(int Id)
        {
            List<OrderModel>? orders = _orders.Where(o => o.CustomerIdFk == Id).ToList() ?? null;
            
            return orders;
        }
        public async Task<OrderModel> ChangeStatusToDone(int Id)
        {
            OrderModel? order = _orders.Include(o => _db.SellerModels).FirstOrDefaultAsync(order => order.Id == Id).Result;


            if (order == null)
            {
                return null;
            }
            else
            {
                order.Status = "Done";
                _orders.Update(order);
                _db.SaveChanges();
                return order;
            }
        }
    }
}
