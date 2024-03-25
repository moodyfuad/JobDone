using JobDone.Models.Customer;
using JobDone.Models.MessageModel;
using JobDone.Models;

namespace JobDone.ViewModels
{
    public class CustomerSellersMessagesServicesViewModel
    {
        public CustomerModel Customer { get; set; }
        public IEnumerable<SellerModel> Sellers { get; set; }
        public IEnumerable<MessageModel> Messages { get; set; }
        public IEnumerable<ServiceModel> Services { get; set; }
    }
}
