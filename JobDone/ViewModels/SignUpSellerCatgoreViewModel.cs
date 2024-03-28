using JobDone.Models.Seller;
using JobDone.Models.SecurityQuestions;
using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.Service;
using System.Drawing;
using JobDone.Models.Order;
using JobDone.Models.Customer;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.SellerAcceptRequest;
using JobDone.Models.Banners;

namespace JobDone.ViewModels
{
    public class SignUpSellerCatgoreViewModel
    {
        public IFormFile PrfilePicture { get; set; }
        public IFormFile PersonalId { get; set; }
        public SellerModel? Seller { get; set; }
        public ServiceModel? Service { get; set; } 
        public List<SecurityQuestionModel>? SecurityQuestions { get; set; }
        public List<CategoryModel>? Category { get; set; }
        public List<OrderModel>? Order { get; set; }
        public List<CustomerModel>? CustomerUsrname { get; set; }
        public List<OrderByCustomerModel>? orderByCustomerModels { get; set; }
        public List<CustomerModel> customerReqwest {  get; set; }
        public IEnumerable<BannerModel> banners {  get; set; }
        public List<SellerAcceptRequestModel> sellerAcceptRequestModels { get; set; }
        internal void CopyTo(MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
