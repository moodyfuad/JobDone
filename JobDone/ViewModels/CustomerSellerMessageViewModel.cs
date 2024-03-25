using JobDone.Models;
using JobDone.Models.Customer;
using JobDone.Models.MessageModel;

namespace JobDone.ViewModels
{
    public class CustomerSellerMessageViewModel
    {
        public CustomerModel Customer { get; set; }
        public SellerModel Seller { get; set; }
        public IEnumerable<MessageModel> Messages { get; set; }
    }
}
