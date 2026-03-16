using Oreon.Domain.Aggregates.Members;

namespace Oreon.Domain.Aggregates.Likes;

public sealed class UserLike
{
    public MemberId SourceMemberId { get; private set; }
    public MemberId TargetMemberId { get; private set; }

    private UserLike() { }

    public UserLike(MemberId sourceMemberId, MemberId targetMemberId)
    {
        if (sourceMemberId == null || sourceMemberId.Value == Guid.Empty)
            throw new ArgumentException("SourceMemberId is required.", nameof(sourceMemberId));
        if (targetMemberId == null || targetMemberId.Value == Guid.Empty)
            throw new ArgumentException("TargetMemberId is required.", nameof(targetMemberId));
        if (sourceMemberId == targetMemberId)
            throw new InvalidOperationException("A member cannot like themselves.");

        SourceMemberId = sourceMemberId;
        TargetMemberId = targetMemberId;
    }
}
