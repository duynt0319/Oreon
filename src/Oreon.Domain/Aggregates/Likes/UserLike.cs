namespace Oreon.Domain.Aggregates.Likes;

public sealed class UserLike
{
    public Guid SourceUserId { get; private set; }
    public Guid TargetUserId { get; private set; }

    private UserLike() { }

    public UserLike(Guid sourceUserId, Guid targetUserId)
    {
        if (sourceUserId == Guid.Empty) throw new ArgumentException("SourceUserId is required.", nameof(sourceUserId));
        if (targetUserId == Guid.Empty) throw new ArgumentException("TargetUserId is required.", nameof(targetUserId));
        if (sourceUserId == targetUserId) throw new InvalidOperationException("A user cannot like themselves.");

        SourceUserId = sourceUserId;
        TargetUserId = targetUserId;
    }
}
