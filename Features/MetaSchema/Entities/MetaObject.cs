using Forge.Shared.Entities;

namespace Forge.Features.MetaSchema.Entities;

public sealed class MetaObject : NamedEntity
{
    public Guid? ObjTypeUid { get; set; }
    
    public Guid ApplicationUid { get; set; }

    public string Version { get; set; } = null!;
}