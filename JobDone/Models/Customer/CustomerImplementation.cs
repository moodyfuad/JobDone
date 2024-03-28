using JobDone.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Security.Claims;
using JobDone.Models.Seller;
namespace JobDone.Models.Customer
{
    public class CustomerImplementation: ICustomer
    {
        private readonly DbSet<CustomerModel> _customer;
        private readonly JobDoneContext _Db;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _host;
        private readonly byte[] DefualtImage = new byte[]
{
    0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x01, 0x01, 0x00, 0x48,
    0x00, 0x48, 0x00, 0x00, 0xFF, 0xDB, 0x00, 0x43, 0x00, 0x08, 0x06, 0x06, 0x07, 0x06, 0x05, 0x08, 0x07,
    0x07, 0x07, 0x09, 0x09, 0x08, 0x0A, 0x0C, 0x14, 0x0D, 0x0C, 0x0B, 0x0B, 0x0C, 0x19, 0x12, 0x13, 0x0F,
    0x14, 0x1D, 0x1A, 0x1F, 0x1E, 0x1D, 0x1A, 0x1C, 0x1C, 0x20, 0x24, 0x2E, 0x27, 0x20, 0x22, 0x2C, 0x23,
    0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C,
    0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C, 0x9C,
    0xFF, 0xC4, 0x00, 0x1F, 0x01, 0x00, 0x03, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B,
    0xFE, 0x00, 0x3B, 0x43, 0x52, 0x45, 0x41, 0x54, 0x4F, 0x52, 0x3A, 0x20, 0x20, 0x4F, 0x70, 0x65, 0x6E,
    0x41, 0x49, 0x20, 0x47, 0x65, 0x6E, 0x65, 0x72, 0x61, 0x74, 0x65, 0x64, 0x20, 0x42, 0x79, 0x20, 0x4F,
    0x70, 0x65, 0x6E, 0x41, 0x49, 0x2E, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

        public CustomerModel Customer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CustomerImplementation(JobDoneContext context, IHostingEnvironment hostingEnvironment) 
        {
            _customer = context.CustomerModels;
            _Db = context;
            _host = hostingEnvironment;
        }

        void SaveCustomerInDB (CustomerModel customer)
        {
            _customer.Add(customer);
            _Db.SaveChanges();
        }

        public CustomerModel GetCustomerById(int id)
        {
            return _customer.FirstOrDefault(s => s.Id == id);
        }

        public CustomerModel getAllInfo(short id)
        {
            var customer = _customer.FirstOrDefault(x => x.Id == id);
            return customer;
        }

        public bool UsernameExist(string username)
        {
            var cu = _customer.Where(c => c.Username == username);
            if (cu.Count() != 0)
            {
                return true;
            }
            else { return false; }
        }

        public short getId(string username, string password)
        {
            var cu = _customer.FirstOrDefault(info => info.Username == username && info.Password == password);
            return (short)cu.Id;
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

        public byte[] ConvertToByteArray(IFormFile image) 
        {
            if (image == null)
            {
                 return DefualtImage;
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyToAsync(memoryStream).Wait();
                    return (memoryStream.ToArray());
                }
            }
            
        }
        public void ApplyChangesToCustomer(ref CustomerModel customer, CustomerModel vmCustomer)
        {
            if (customer.Username != vmCustomer.Username)
            {
                customer.Username = vmCustomer.Username;
            }

            if (customer.FirstName != vmCustomer.FirstName)
            {
                customer.FirstName = vmCustomer.FirstName;
            }

            if (customer.LastName != vmCustomer.LastName)
            {
                customer.LastName = vmCustomer.LastName;
            }

            if (customer.Email != vmCustomer.Email)
            {
                customer.Email = vmCustomer.Email;
            }

            if (customer.PhoneNumber != vmCustomer.PhoneNumber)
            {
                customer.PhoneNumber = vmCustomer.PhoneNumber;
            }

            if (customer.BirthDate != vmCustomer.BirthDate)
            {
                customer.BirthDate = vmCustomer.BirthDate;
            }

        }

        public async Task<decimal> GetWalletAmount(short id)
        {
            var customer = await _customer.FirstOrDefaultAsync(w => w.Id == id);
            return customer.Wallet;
        }

        public async Task<string> GetPictureAsString(short id)
        {
            var customer = await _customer.FirstOrDefaultAsync(c => c.Id == id);
            byte[] pictureData = customer.ProfilePicture;

            string ProfilePic = Convert.ToBase64String(pictureData);

            return ProfilePic;
        }
        public async Task<List<CustomerModel>>? GetCustomerWithRequests(int customerId)
        {
            List<CustomerModel> customers =
                _customer.Where(c => c.Id == customerId).Include(s => s.OrderByCustomerModels).ToList();

            return customers;
        }
        public List<CustomerModel> GetFirst10(string username)
        {
            List<CustomerModel> customers = _Db.CustomerModels.Where(s =>
                s.Username.ToLower().StartsWith(username.ToLower()) || s.Username.ToLower().EndsWith(username.ToLower())).ToList();
            if (customers.Count == 0)
            {
                return null;
            }
            else if (customers.Count == 1)
            {
                customers.First().OrderByCustomerModels = _Db.OrderByCustomerModels.Where(p => p.CustomerIdFk == customers.First().Id).ToList();
                return customers;
            }
            else if (customers.Count < 10)
            {
                return customers;
            }
            else
            {
                return customers.Take(10).ToList();
            }
        }

        public async Task<bool> DeleteAccount(int customerId)
        {
            try
            {
                CustomerModel? customer = _customer.
                                              Include(s => s.OrderByCustomerModels).
                                              Include(s => s.OrderModels).
                                              Include(s => s.SecurityQuestionIdFkNavigation).                                         FirstOrDefaultAsync(s => s.Id == customerId).Result;

                try
                {
                    _Db.CustomerModels.Remove(customer);
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

        public async Task<CustomerModel> Withdraw(CustomerModel customer, decimal MoneyAmount)
        {
            customer.Wallet -= MoneyAmount;
            try
            {
                _customer.Update(customer);
                _Db.SaveChanges();

                return customer;
            }
            catch
            {
                return null;
            }

        }
        public async Task<CustomerModel> Diposit(CustomerModel customer, decimal MoneyAmount)
        {
            customer.Wallet += MoneyAmount;
            try
            {
                _customer.Update(customer);
                _Db.SaveChanges();

                return customer;
            }
            catch
            {
                return null;
            }
        }
    }
}
