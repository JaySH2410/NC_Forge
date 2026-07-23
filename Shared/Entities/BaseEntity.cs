namespace test.Shared.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public Guid Uuid { get; set; } = Guid.CreateVersion7();
}