using Oreon.Application.Common.Pagination;
using Oreon.Application.DTOs;
using Oreon.Domain.Aggregates.Messages;
using Oreon.Domain.Aggregates.Presence;

namespace Oreon.Application.Abstractions.Persistence;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void RemoveMessage(Message message);
    Task<Message> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams, CancellationToken ct = default);
    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername, CancellationToken ct = default);
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection> GetConnectionAsync(string connectionId, CancellationToken ct = default);
    Task<Group> GetMessageGroupAsync(string groupName, CancellationToken ct = default);
    Task<Group> GetGroupForConnectionAsync(string connectionId, CancellationToken ct = default);
}
