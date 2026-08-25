namespace Forge.Shared.Identifiers;

public interface IForgeUuidGenerator
{
    Task<Guid> GenerateApplicationUuidAsync(
        CancellationToken cancellationToken = default);

    Task<Guid> GenerateMetaObjectUuidAsync(
        CancellationToken cancellationToken = default);

    Task<Guid> GenerateRelationshipUuidAsync(
        CancellationToken cancellationToken = default);
}