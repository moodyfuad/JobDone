using JobDone.Data;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Customer
{
    public class CustomerImplementation: ICustomer
    {
        private readonly DbSet<CustomerModel> _customer;
        private readonly JobDoneContext _Db;

        public CustomerImplementation(JobDoneContext context) 
        {
            _customer = context.CustomerModels;
            _Db = context;
        }

        void SaveCustomerInDB (CustomerModel customer)
        {
            _customer.Add(customer);
            _Db.SaveChanges();
        }

        public bool UsernameExist(CustomerModel customer)
        {
            var cu = _customer.Where(c => c.Username == customer.Username);
            if (cu == null)
            {
                return true;
            }
            else { return false; }
        }

        public bool UsernameAndPasswordExists(CustomerModel customer)
        {
            var cu = _customer.FirstOrDefault(c => c.Username == customer.Username && c.Password == customer.Password);
            if (cu != null)
            {
                return true;
            }
            else { return false; }

        }

        public void SignUp(CustomerModel customer)
        {
               SaveCustomerInDB(customer);
        }
    }
}
