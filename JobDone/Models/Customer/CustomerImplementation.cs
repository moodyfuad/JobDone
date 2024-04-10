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
        private byte[] GetDefualtImage()
        {
            string rootPath = _host.WebRootPath;
            string imagePath = Path.Combine(rootPath, "Customer/Picture", "ProfilePicture.jpg");

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

            return imageBytes;
        }
        
        
        
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

        public async Task SignUp(CustomerModel customer)
        {
            await _customer.AddAsync(customer);
            await _Db.SaveChangesAsync();
        }

        public byte[] ConvertToByteArray(IFormFile image) 
        {
            if (image == null)
            {
                 return GetDefualtImage();
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

        public async Task<IEnumerable<CustomerModel>> GetAllCustomers()
        {
            return await _customer.ToListAsync();
        }
    }
}
