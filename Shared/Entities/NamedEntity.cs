namespace test.Shared.Entities;

public abstract class NamedEntity : ActivatableEntity
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}