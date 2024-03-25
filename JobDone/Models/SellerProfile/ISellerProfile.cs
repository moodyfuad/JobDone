using JobDone.Models.Customer;

namespace JobDone.Models.SellerProfile
{
    public interface ISellerProfile
    {
        SellerModel GetSellerProfile(int sellerID);
        bool UsernameExist(string username);
        public byte[] ConvertToByteArray(IFormFile image);
        public void ApplyChangesToCustomer(ref SellerModel Seller, SellerModel ViewModelSeller);

    }
}
