using Oreon.Domain.DomainEvents;
using Oreon.Domain.Shared;

namespace Oreon.Domain.Aggregates.Messages;

public sealed class Message : AggregateRoot<MessageId>
{
    public Guid SenderId { get; private set; }
    public string SenderUsername { get; private set; }
    public Guid RecipientId { get; private set; }
    public string RecipientUsername { get; private set; }
    public string Content { get; private set; }
    public DateTime? DateRead { get; private set; }
    public DateTime MessageSent { get; private set; }
    public bool SenderDeleted { get; private set; }
    public bool RecipientDeleted { get; private set; }

    private Message() { }

    public static Message Create(
        Guid senderId,
        string senderUsername,
        Guid recipientId,
        string recipientUsername,
        string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Message content cannot be empty.", nameof(content));
        if (senderId == recipientId)
            throw new InvalidOperationException("A user cannot send a message to themselves.");

        var message = new Message
        {
            SenderId = senderId,
            SenderUsername = senderUsername,
            RecipientId = recipientId,
            RecipientUsername = recipientUsername,
            Content = content,
            MessageSent = DateTime.UtcNow
        };

        message.AddDomainEvent(new MessageSentDomainEvent(senderId, recipientId, DateTimeOffset.UtcNow));
        return message;
    }

    public void MarkAsRead()
    {
        if (DateRead is null)
            DateRead = DateTime.UtcNow;
    }

    public void DeleteForSender() => SenderDeleted = true;

    public void DeleteForRecipient() => RecipientDeleted = true;
}
