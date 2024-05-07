namespace JobDone.Models.Customer
{
    public interface ICustomer
    {
        Task SignUp(CustomerModel customer);
        bool UsernameExist(string customer) ;
        bool UsernameAndPasswordExists(CustomerModel customer);
        public byte[] ConvertToByteArray(IFormFile image);

        CustomerModel getAllInfo(short id);

        short getId(string username, string password);
        public void ApplyChangesToCustomer(ref CustomerModel Customer, CustomerModel vmCustomer);

        public Task<decimal> GetWalletAmount(short id);

        public Task<string> GetPictureAsString(short id);
        public CustomerModel GetCustomerById(int id);

        Task<List<CustomerModel>>? GetCustomerWithRequests(int customerId);
        List<CustomerModel> GetFirst10(string username);
        Task<bool> DeleteAccount(int customerId);
        Task<CustomerModel> Withdraw(CustomerModel customer, decimal MoneyAmount);
        Task<CustomerModel> Diposit(CustomerModel customer, decimal MoneyAmount);

        public Task<IEnumerable<CustomerModel>> GetAllCustomers();
        public Task UpdateTheInfo(CustomerModel customer);
    }
}
