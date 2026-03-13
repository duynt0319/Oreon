using Oreon.Domain.Shared;

namespace Oreon.Domain.DomainEvents;

public sealed record MessageSentDomainEvent(Guid SenderId, Guid RecipientId, DateTimeOffset OccurredAt) : IDomainEvent;
