using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Entities;

namespace Domain.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;

    public MessageService(IMessageRepository messageRepository)
    {
        _repository = messageRepository;
    }

    public async Task<Message> GetMessageById(string id) => await _repository.GetById(id);

    public async Task<List<Message>> ListMessages() => await _repository.List();

    public async Task AddMessage(Message message) => await _repository.Add(message);

    public async Task DeleteMessage(string id) => await _repository.Delete(id);
}
