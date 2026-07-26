namespace Forge.Shared.Entities;

public abstract class AuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    
    public string? DeletedBy { get; set; }
}