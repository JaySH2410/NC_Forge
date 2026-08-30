using Forge.Shared.Entities;

namespace Forge.Features.MetaSchema.Entities;

public sealed class MetaInterface : ActivatableEntity
{
    public Guid ObjUid { get; set; }
    public Guid InterfaceUid { get; set; }
    public bool IsPrimary { get; set; }
    public int Ordinal { get; set; }
}