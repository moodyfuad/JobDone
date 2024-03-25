using JobDone.Models.Customer;

namespace JobDone.Models.SellerProfile
{
    public interface ISellerProfile
    {
        SellerModel GetSellerProfile(int sellerID);
        List<ServiceModel> GetServiceModels(int sellerID);
        bool IsWithdrawAmountbefore(int sellerID);
        void AddWithdrawMoney(WithdrawModel AWDM);
        bool UsernameExist(string username);
        public byte[] ConvertToByteArray(IFormFile image);
        public void ApplyChangesToCustomer(ref SellerModel Seller, SellerModel ViewModelSeller);

    }
}
