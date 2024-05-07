namespace JobDone.Models.MessageModel
{
    public interface IMessage
    {
        public Task<IEnumerable<MessageModel>> GetAllMessages(short customerId, short sellerId);
        public Task<IEnumerable<MessageModel>> GetAllMessages();
        public Task DeleteAllMessagesBetweenCustomerAndSeller(short customerId, short sellerId);
    }
}
