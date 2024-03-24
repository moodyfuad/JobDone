using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.SellerAcceptRequest;
using JobDone.ViewModels;
using System.Drawing;

namespace JobDone.Models.Seller
{
    public interface ISeller
    {
        void SignUp(SellerModel seller);
        bool UsernameExist(SellerModel seller);
        bool CheckUsernameAndPasswordExists(SellerModel seller);
        byte[] ConvertToByte(IFormFile image);
        public IFormFile ConvertToImage(byte[] byteImage);

        SellerModel GetSellerById(int id);
        short getId(string username, string password);
        decimal GetWallet(int id);
        int GetRemainingWork(int id);
        List<SellerModel> GetSellersWhoAcceptedRequest(List<int> sellersId);
        int AveilabelRReqest(int sellerId);
        Decimal Totalgains(int sellerId);
        string OrderName(int sellerId);
        int OrderCount(int sellerId);
        List<OrderModel> orderModels(int sellerId);
        List<CustomerModel> GetCustomerusername(int sellerId);
        List<OrderByCustomerModel> GetOrderByCustomerModels(int sellerCatgoreId);
        int SellerCatgoreID(int sellerId);
        List<CustomerModel> CustomerReqwestWork(int SellerID);
        void SaveSellerAccept(SellerAcceptRequestModel SAR);
        List<SellerAcceptRequestModel> GetSellerAcceptRequestModels();
        public Task<IEnumerable<SellerModel>> GetAllSellersWithCategory(string search);
        public Task<IEnumerable<SellerModel>> getAllSelersBasedOnUsername(string search);
        public Task<IEnumerable<ServiceModel>> GetAllSellersWithService(string search);
        public Task<IEnumerable<SellerModel>> getAllTheSeller();
    }
}
