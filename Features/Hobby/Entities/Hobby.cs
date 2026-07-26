using Forge.Shared.Entities;

namespace Forge.Features.Hobbies.Entities;

public class Hobby : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}