using Oreon.Domain.Shared;

namespace Oreon.Domain.DomainEvents;

public sealed record LikeAddedDomainEvent(Guid SourceMemberId, Guid TargetMemberId, DateTimeOffset OccurredAt) : IDomainEvent;
