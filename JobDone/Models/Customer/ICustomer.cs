namespace JobDone.Models.Customer
{
    public interface ICustomer
    {
        void SignUp(CustomerModel customer) ;
        bool UsernameExist(CustomerModel customer) ;
        bool UsernameAndPasswordExists(CustomerModel customer);
    }
}
