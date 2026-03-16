namespace Oreon.Domain.Aggregates.Messages;

public sealed record MessageId(Guid Value)
{
    public static MessageId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("MessageId cannot be empty.", nameof(value));
        return new MessageId(value);
    }

    public static MessageId NewId() => new(Guid.NewGuid());
}
