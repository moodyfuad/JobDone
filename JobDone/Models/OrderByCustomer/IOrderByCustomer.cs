using JobDone.Data;

namespace JobDone.Models.OrderByCustomer
{
    public interface IOrderByCustomer
    {
        void PostRequest(OrderByCustomerModel request);
        int GetCustomerId(string username);
    }
}
