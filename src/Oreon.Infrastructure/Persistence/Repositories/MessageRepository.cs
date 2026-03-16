//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Oreon.Application.Abstractions.Persistence;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using Oreon.Domain.Messages;
//using Oreon.Domain.Presence;
//using Microsoft.EntityFrameworkCore;

//namespace Oreon.Infrastructure.Persistence.Repositories;

//public sealed class MessageRepository : IMessageRepository
//{
//    private readonly DataContext _context;
//    private readonly IMapper _mapper;

//    public MessageRepository(DataContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public void AddMessage(Message message) => _context.Messages.Add(message);

//    public void RemoveMessage(Message message) => _context.Messages.Remove(message);

//    public async Task<Message> GetByIdAsync(int id, CancellationToken ct = default)
//        => await _context.Messages.FindAsync(new object[] { id }, ct);

//    public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams, CancellationToken ct = default)
//    {
//        var query = _context.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();

//        query = messageParams.Container switch
//        {
//            "Inbox" => query.Where(m => m.RecipientUsername == messageParams.Username && !m.RecipientDeleted),
//            "Outbox" => query.Where(m => m.SenderUsername == messageParams.Username && !m.SenderDeleted),
//            _ => query.Where(m => m.RecipientUsername == messageParams.Username && m.DateRead == null && !m.RecipientDeleted)
//        };

//        var projectedQuery = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
//        var count = await projectedQuery.CountAsync(ct);
//        var items = await projectedQuery
//            .Skip((messageParams.PageNumber - 1) * messageParams.PageSize)
//            .Take(messageParams.PageSize)
//            .ToListAsync(ct);
//        return new PagedList<MessageDto>(items, count, messageParams.PageNumber, messageParams.PageSize);
//    }

//    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername, CancellationToken ct = default)
//    {
//        var messages = await _context.Messages
//            .Include(m => m.Sender).ThenInclude(u => u.Photos)
//            .Include(m => m.Recipient).ThenInclude(u => u.Photos)
//            .Where(m =>
//                m.RecipientUsername == currentUsername && !m.RecipientDeleted && m.SenderUsername == recipientUsername ||
//                m.RecipientUsername == recipientUsername && !m.SenderDeleted && m.SenderUsername == currentUsername)
//            .OrderBy(m => m.MessageSent)
//            .ToListAsync(ct);

//        var unread = messages
//            .Where(m => m.DateRead == null && m.RecipientUsername == currentUsername)
//            .ToList();

//        foreach (var message in unread)
//            message.MarkAsRead();

//        if (unread.Count != 0)
//            await _context.SaveChangesAsync(ct);

//        return _mapper.Map<IEnumerable<MessageDto>>(messages);
//    }

//    public void AddGroup(Group group) => _context.Groups.Add(group);

//    public void RemoveConnection(Connection connection) => _context.Connections.Remove(connection);

//    public async Task<Connection> GetConnectionAsync(string connectionId, CancellationToken ct = default)
//        => await _context.Connections.FindAsync(new object[] { connectionId }, ct);

//    public async Task<Group> GetMessageGroupAsync(string groupName, CancellationToken ct = default)
//        => await _context.Groups
//            .Include(g => g.Connections)
//            .FirstOrDefaultAsync(g => g.Name == groupName, ct);

//    public async Task<Group> GetGroupForConnectionAsync(string connectionId, CancellationToken ct = default)
//        => await _context.Groups
//            .Include(g => g.Connections)
//            .FirstOrDefaultAsync(g => g.Connections.Any(c => c.ConnectionId == connectionId), ct);
//}
