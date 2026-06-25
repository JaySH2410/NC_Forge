namespace test.Shared.Entities;

public abstract class NamedEntity : BaseEntity
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}