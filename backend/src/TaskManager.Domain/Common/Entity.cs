namespace TaskManager.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected Entity(Guid id) => Id = id;
    protected Entity() => Id = Guid.NewGuid();

    public override bool Equals(object? obj) => obj is Entity e && e.Id == Id;
    public override int GetHashCode() => Id.GetHashCode();
}
