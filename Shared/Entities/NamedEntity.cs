namespace Forge.Shared.Entities;

public abstract class NamedEntity : ActivatableEntity
{
    public required string Name { get; set; }

    public string? DisplayName { get; set; } 

    public string? Description { get; set; }
}