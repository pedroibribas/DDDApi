using Domain.Interfaces.Repositories;
using Entities;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
    }
}
