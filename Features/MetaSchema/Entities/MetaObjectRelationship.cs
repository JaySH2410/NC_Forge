using Forge.Shared.Entities;

namespace Forge.Features.MetaSchema.Entities;

public sealed class MetaObjectRelationship : NamedEntity
{
    public Guid End1Uid { get; set; }

    public Guid End2Uid { get; set; }

    public Guid RelTypeUid { get; set; }

    public int Ordinal { get; set; }
}