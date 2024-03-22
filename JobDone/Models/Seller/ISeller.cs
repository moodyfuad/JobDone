using JobDone.Models.Customer;
using JobDone.Models.Order;
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
        int AveilabelRReqest(int sellerId);
        Decimal Totalgains(int sellerId);
        string OrderName(int sellerId);
        int OrderCount(int sellerId);
        List<OrderModel> orderModels(int sellerId);
        int customerID(int ordrId);

        public Task<IEnumerable<SellerModel>> getAllTheSeller();
    }
}
