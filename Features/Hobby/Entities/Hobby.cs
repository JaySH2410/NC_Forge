using test.Shared.Entities;

namespace test.Features.Hobbies.Entities;

public class Hobby : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}