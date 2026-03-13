namespace Oreon.Domain.Aggregates.Members;

public sealed record MemberId(Guid Value)
{
    public static MemberId Of(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("MemberId cannot be empty.", nameof(value));
        return new MemberId(value);
    }

    public static MemberId NewId() => new(Guid.NewGuid());
}
