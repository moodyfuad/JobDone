namespace JobDone.Models.Order
{
    public interface IOrder
    {
        List<OrderModel> GetCustomerOrders(int Id);

        Task<OrderModel> ChangeStatusToDone(int Id);
        public int GetCustomerIdFromOrder(int orderByCustomerId);
    }
}
