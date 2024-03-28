using JobDone.Models.Customer;
using JobDone.Models.MessageModel;
using JobDone.Models;

namespace JobDone.ViewModels
{
    public class CustomersSellerMessagesViewModel
    {
        public IEnumerable<CustomerModel> Customers { get; set; }
        public SellerModel Seller { get; set; }
        public IEnumerable<MessageModel> Messages { get; set; }
    }
}
