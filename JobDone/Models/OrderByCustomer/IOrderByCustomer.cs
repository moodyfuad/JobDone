using JobDone.Data;

namespace JobDone.Models.OrderByCustomer
{
    public interface IOrderByCustomer
    {
        void PostRequest(OrderByCustomerModel request);
        int GetCustomerId(string username);
        Task<List<OrderByCustomerModel>?> GetOrdersByCustomerId(int CustomerId);
        Task<OrderByCustomerModel> DeleteOrder(int OrderId);
    }
}
