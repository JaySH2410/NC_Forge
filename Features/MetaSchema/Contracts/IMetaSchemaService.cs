using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaService
{
    Task<MetaObject?> GetObjectAsync(
      Guid objUid,
      CancellationToken cancellationToken = default);

    Task<MetaObjectRelationship?> GetRelationshipAsync(
      Guid relUid,
      CancellationToken cancellationToken = default);

    Task<MetaObject?> GetObjectByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<MetaObjectRelationship?> GetRelationshipByNameAsync(
       string name,
       CancellationToken cancellationToken = default);
        Guid objUid,
        CancellationToken cancellationToken = default);

    Task<bool> RelationshipExistsAsync(
       Guid relUid,
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

    Task<MetaObject?> GetSingleReferencingObjectAsync(
       Guid targetUid,
       Guid relationshipTypeUid,
       CancellationToken cancellationToken = default);
}