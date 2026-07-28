using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaService
{
    Task<MetaObject?> GetObjectAsync(
      Guid objUid,
      CancellationToken cancellationToken = default);

    Task<MetaObject?> GetObjectByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid objUid,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MetaObject>> GetRelatedObjectsAsync(
        Guid sourceUid,
        Guid relationshipTypeUid,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MetaObject>> GetReferencingObjectsAsync(
        Guid targetUid,
        Guid relationshipTypeUid,
        CancellationToken cancellationToken = default);

    Task<MetaObject?> GetSingleRelatedObjectAsync(
        Guid sourceUid,
        Guid relationshipTypeUid,
        CancellationToken cancellationToken = default);
}