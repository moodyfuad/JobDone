using JobDone.Data;
using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;

namespace JobDone.ViewModels
{
    public class RequestListViewModel
    {
        public OrderByCustomerModel? orderByCustomer {  get; set; }
        public List<SellerModel>? sellers { get; set; }
    }
}
