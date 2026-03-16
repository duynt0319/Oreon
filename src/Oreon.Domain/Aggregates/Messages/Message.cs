using Oreon.Domain.Aggregates.Members;
using Oreon.Domain.DomainEvents;
using Oreon.Domain.Shared;

namespace Oreon.Domain.Aggregates.Messages;

public sealed class Message : AggregateRoot<MessageId>
{
    public MemberId SenderMemberId { get; private set; }
    public string SenderUsername { get; private set; }
    public MemberId RecipientMemberId { get; private set; }
    public string RecipientUsername { get; private set; }
    public string Content { get; private set; }
    public DateTime? DateRead { get; private set; }
    public DateTime MessageSent { get; private set; }
    public bool SenderDeleted { get; private set; }
    public bool RecipientDeleted { get; private set; }

    private Message() { }

    public static Message Create(
        MemberId senderMemberId,
        string senderUsername,
        MemberId recipientMemberId,
        string recipientUsername,
        string content
    )
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Message content cannot be empty.", nameof(content));
        if (senderMemberId == recipientMemberId)
            throw new InvalidOperationException("A member cannot send a message to themselves.");

        var message = new Message
        {
            SenderMemberId = senderMemberId,
            SenderUsername = senderUsername,
            RecipientMemberId = recipientMemberId,
            RecipientUsername = recipientUsername,
            Content = content,
            MessageSent = DateTime.UtcNow,
        };

        message.AddDomainEvent(
            new MessageSentDomainEvent(senderMemberId, recipientMemberId, DateTimeOffset.UtcNow)
        );
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
