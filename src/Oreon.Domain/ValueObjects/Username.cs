using Oreon.Domain.Shared;

namespace Oreon.Domain.ValueObjects;

public sealed class Username : ValueObject
{
    public string Value { get; }

    private Username(string value) => Value = value;

    public static Username Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Username cannot be empty.", nameof(value));
        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException(
                "Username must be between 3 and 50 characters.",
                nameof(value)
            );
        return new Username(value.Trim().ToLowerInvariant());
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
