namespace JobDone.Models.Customer
{
    public interface ICustomer
    {
        void SignUp(CustomerModel customer) ;
        bool UsernameExist(string customer) ;
        bool UsernameAndPasswordExists(CustomerModel customer);
        public byte[] ConvertToByteArray(IFormFile image);

        CustomerModel getAllInfo(short id);

        short getId(string username, string password);
        public void ApplyChangesToCustomer(ref CustomerModel Customer, CustomerModel vmCustomer);

        public Task<decimal> GetWalletAmount(short id);

        public Task<string> GetPictureAsString(short id);
        public CustomerModel GetCustomerById(int id);

    }
}
