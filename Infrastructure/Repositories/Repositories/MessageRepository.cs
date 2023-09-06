using Domain.Interfaces.Repositories;
using Entities;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public List<Message> ListMessagesByUserId(string userId)
        {
            return _db.Set<Message>()
                .Where(m => m.UserId!.Contains(userId))
                .ToList();
        }
    }
}
