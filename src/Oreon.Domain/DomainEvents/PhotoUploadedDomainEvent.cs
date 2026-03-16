using Oreon.Domain.Aggregates.Members;
using Oreon.Domain.Shared;

namespace Oreon.Domain.DomainEvents;

public sealed record PhotoUploadedDomainEvent(MemberId MemberId, DateTimeOffset OccurredAt)
    : IDomainEvent;
