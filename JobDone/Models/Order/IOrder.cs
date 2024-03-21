namespace JobDone.Models.Order
{
    public interface IOrder
    {
        List<OrderModel> GetCustomerOrders(int Id);
    }
}
