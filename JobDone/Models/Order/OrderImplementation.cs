using JobDone.Data;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Order
{
    public class OrderImplementation : IOrder
    {
        private readonly DbSet<OrderModel> _orders;

        public OrderImplementation(JobDoneContext context)
        {
            _orders = context.OrderModels;
        }
        
        public List<OrderModel>? GetCustomerOrders(int Id)
        {
            List<OrderModel>? orders = _orders.Where(o => o.CustomerIdFk == Id).ToList() ?? null;
            
            return orders;
        }
    }
}
