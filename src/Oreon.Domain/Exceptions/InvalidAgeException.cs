namespace Oreon.Domain.Exceptions;

public sealed class InvalidAgeException : DomainException
{
    public InvalidAgeException()
        : base("member.invalid_age", "Member must be at least 18 years old.") { }
}
