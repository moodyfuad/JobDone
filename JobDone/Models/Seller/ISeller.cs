using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.SellerAcceptRequest;
using JobDone.Models.SellerOldWork;
using JobDone.ViewModels;
using System.Drawing;

namespace JobDone.Models.Seller
{
    public interface ISeller
    {
        void SignUp(SellerModel seller);
        bool UsernameExist(string username);
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
        void ChangeOrderStatus(int orderID);
        void DeleteOrder(int orderID);
        int OrderCount(int sellerId);
        List<OrderModel> orderModels(int sellerId);
        List<CustomerModel> GetCustomerusername();
        List<OrderByCustomerModel> GetOrderByCustomerModels(int sellerCatgoreId, int sellerId);
        public Task<List<int>> GetRequestsThatTheSellerAccept(int sellerId);
        List<OrderByCustomerModel> GetOrderByCustomerModelsFiveCustomer(int sellerCatgoreId);
        int SellerCatgoreID(int sellerId);
        List<CustomerModel> CustomerReqwestWork(int SellerID);
        void SaveSellerAccept(SellerAcceptRequestModel SAR);
        int GetSARMForOneSeller(int sellerId);
        List<SellerAcceptRequestModel> GetSellerAcceptRequestModels();
        List<OrderByCustomerModel> getAllOrderByCustomerBasedOnOrdername(string search, int sellerId);
        public Task<IEnumerable<SellerModel>> GetAllSellersWithCategory(string search);
        public Task<IEnumerable<SellerModel>> getAllSelersBasedOnUsername(string search);
        public Task<IEnumerable<ServiceModel>> GetAllSellersWithService(string search);
        public Task<IEnumerable<SellerModel>> getAllTheSeller();
        public Task<IEnumerable<SellerModel>> GetSellers(int number);

        List<SellerModel> GetFirst10();
        List<SellerModel> GetFirst10(string username);
        Task<bool> DeleteAccount(int sellerId);
        public Task<List<SellerModel>>? GetSellerWithPosts(int sellerId);
        Task<SellerModel> Withdraw(SellerModel seller ,decimal MoneyAmount);
        Task<SellerModel> Diposit(SellerModel seller ,decimal MoneyAmount);
        Task<SellerModel> LikeSellerByUsername(string sellerUsername);
    }
}
