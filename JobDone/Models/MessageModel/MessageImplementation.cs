using JobDone.Data;
using JobDone.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.MessageModel
{
    public class MessageImplementation : IMessage
    {
        private readonly JobDoneContext _context;
        private readonly DbSet<MessageModel> _message;

        public MessageImplementation(JobDoneContext context)
        {
            _context = context;
            _message = context.MessageModels;
        }

        private async Task<IEnumerable<MessageModel>> _GetAllMessagesBetweenCustomerAndSeller(short customerId, short sellerId) => await _message.Where(x => x.CustomerId == customerId && x.SellerId == sellerId).ToListAsync();
        public async Task DeleteAllMessagesBetweenCustomerAndSeller(short customerId, short sellerId)
        {
            IEnumerable<MessageModel> messages = await _GetAllMessagesBetweenCustomerAndSeller(customerId, sellerId);
            foreach (MessageModel message in messages)
            {
                _message.Remove(message);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageModel>> GetAllMessages(short customerId, short sellerId) => await _message.Where(m => m.CustomerId == customerId && m.SellerId == sellerId).ToListAsync();

        public async Task<IEnumerable<MessageModel>> GetAllMessages() => await _message.ToListAsync();

        public async Task AddMessage(MessageModel message)
        {
            _message.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
