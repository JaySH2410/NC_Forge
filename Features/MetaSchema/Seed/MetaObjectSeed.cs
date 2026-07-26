namespace Forge.Features.MetaSchema.Seed;

public sealed record MetaObjectSeed(
    Guid Uuid,
    string Name,
    string DisplayName);