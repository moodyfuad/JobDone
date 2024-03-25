namespace JobDone.Models.MessageModel
{
    public interface IMessage
    {
        public Task<IEnumerable<MessageModel>> GetAllMessages();
    }
}
