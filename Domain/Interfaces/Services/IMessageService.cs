using Entities;

namespace Domain.Interfaces.Services;

public interface IMessageService
{
    Task<Message> GetMessageById(int id);
    Task<List<Message>> ListMessages();
    Task AddMessage(Message message);
    Task DeleteMessage(string id);
    List<Message> ListMessagesByUserId(string userId);
}
