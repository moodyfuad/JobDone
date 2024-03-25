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
            _message = context.MessageModels;
        }

        public async Task<IEnumerable<MessageModel>> GetAllMessages() => await _message.ToListAsync();
    }
}
