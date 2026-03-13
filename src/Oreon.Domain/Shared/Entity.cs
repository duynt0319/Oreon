namespace Oreon.Domain.Shared;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
{
    public TId Id { get; protected set; } = default!;

    public bool Equals(Entity<TId> other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id?.Equals(other.Id) ?? false;
    }

    public override bool Equals(object obj) => obj is Entity<TId> entity && Equals(entity);

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => left?.Equals(right) ?? right is null;

    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !(left == right);
}
