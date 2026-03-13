namespace Oreon.Domain.Shared;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
