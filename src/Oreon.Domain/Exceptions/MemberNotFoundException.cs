namespace Oreon.Domain.Exceptions;

public sealed class MemberNotFoundException : DomainException
{
    public MemberNotFoundException(string username)
        : base("member.not_found", $"Member '{username}' was not found.") { }

    public MemberNotFoundException(int id)
        : base("member.not_found", $"Member with id '{id}' was not found.") { }
}
