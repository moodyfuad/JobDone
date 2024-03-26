using JobDone.Models.Customer;
using JobDone.Models.SellerOldWork;

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
        List<SellerOldWorkModel> GetSellerOldWorkModels(int sellerID);
        void DeleteOldWork(int oldWorkId);
        void editOldWork(int oldworkId, IFormFile newphoto, string newdescrepion);
        SellerOldWorkModel GetOneSellerOldWorkModel(int oldWorkId);
        void AddNewWork(IFormFile imge, string description, int sellerId);

    }
}
