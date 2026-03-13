using Oreon.Domain.Shared;
using System.Text.RegularExpressions;

namespace Oreon.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException($"'{value}' is not a valid email address.", nameof(value));
        return new Email(value.Trim().ToLowerInvariant());
    }

    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }

    public override string ToString() => Value;
}
