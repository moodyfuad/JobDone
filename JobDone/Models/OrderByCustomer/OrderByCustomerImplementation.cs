using JobDone.Data;
using JobDone.Models.Customer;
using SQLitePCL;

namespace JobDone.Models.OrderByCustomer
{
    public class OrderByCustomerImplementation : IOrderByCustomer
    {
        private readonly JobDoneContext _context;

        public OrderByCustomerImplementation(JobDoneContext context) 
        {
            _context = context;
        }

        public int GetCustomerId(string? username)
        {
            if (username == null) return 0;
            int customerId = 0;
            CustomerModel? customer = _context.CustomerModels.FirstOrDefault(c => c.Username == username);
            if(customer != null)
            {
                customerId = customer.Id;
                return customerId;
            }
            return 0;
        }

        public void PostRequest(OrderByCustomerModel request)
        {
            _context.Add(request);
            _context.SaveChanges();
        }
    }
}
