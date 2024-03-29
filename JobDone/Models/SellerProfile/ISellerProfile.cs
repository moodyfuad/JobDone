using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.SellerOldWork;
using JobDone.Models.Withdraw;

namespace JobDone.Models.SellerProfile
{
    public interface ISellerProfile
    {
        SellerModel GetSellerProfile(int sellerID);
        List<CategoryModel> GetCategories();
        List<ServiceModel> GetServiceModels(int sellerID);
        bool IsWithdrawAmountbefore(int sellerID);
        void AddWithdrawMoney(WithdrawModel AWDM);
        List<WithdrawModel> GetAllwithdrawForOneSeller(int sellerId);
        bool UsernameExist(string username);
        public byte[] ConvertToByteArray(IFormFile image);
        public void ApplyChangesToSeller(ref SellerModel Seller, SellerModel ViewModelSeller);
        List<SellerOldWorkModel> GetSellerOldWorkModels(int sellerID);
        void DeleteOldWork(int oldWorkId);
        void editOldWork(int oldworkId, IFormFile newphoto, string newdescrepion);
        SellerOldWorkModel GetOneSellerOldWorkModel(int oldWorkId);
        void AddNewWork(IFormFile imge, string description, int sellerId);
        public ServiceModel GetServiceInfo(ServiceModel service);

    }
}
