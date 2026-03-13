namespace Oreon.Domain.Aggregates.Members;

public sealed record PhotoId(Guid Value)
{
    public static PhotoId Of(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("PhotoId cannot be empty.", nameof(value));
        return new PhotoId(value);
    }

    public static PhotoId NewId() => new(Guid.NewGuid());
}
