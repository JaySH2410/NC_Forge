namespace Forge.Features.MetaSchema.Seed;

public sealed record MetaObjectRelationshipSeed(
    Guid End1Uid,
    Guid End2Uid,
    int Ordinal,
    Guid Uuid,
    string Name,
    string DisplayName,
    Guid RelTypeUid
    );