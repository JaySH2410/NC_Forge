namespace Forge.Shared.Entities;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
}