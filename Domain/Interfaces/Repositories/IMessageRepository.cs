using Domain.Interfaces.Repositories.Base;
using Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        List<Message> ListMessagesByUserId(string userId);
    }
}
