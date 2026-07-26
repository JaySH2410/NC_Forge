using Forge.Shared.Entities;

namespace Forge.Features.MetaSchema.Entities;

public class Application: NamedEntity
{
    public string Version { get; set; } = null!;
}