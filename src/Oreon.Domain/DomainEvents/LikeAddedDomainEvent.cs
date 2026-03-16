using Oreon.Domain.Aggregates.Members;
using Oreon.Domain.Shared;

namespace Oreon.Domain.DomainEvents;

public sealed record LikeAddedDomainEvent(
    MemberId SourceMemberId,
    MemberId TargetMemberId,
    DateTimeOffset OccurredAt
) : IDomainEvent;
