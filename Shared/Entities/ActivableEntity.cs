namespace Forge.Shared.Entities;

public abstract class ActivatableEntity : AuditableEntity
{
    public bool IsActive { get; set; } = true;
}