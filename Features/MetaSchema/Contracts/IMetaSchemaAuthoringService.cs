using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaAuthoringService
{
    Task<MetaObject> CreateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task<MetaObject> UpdateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task DeactivateObjectAsync(
        Guid objUid,
        CancellationToken cancellationToken = default);

    Task TerminateObjectAsync(
        Guid objUid,
        CancellationToken cancellationToken = default);

    Task<MetaObjectRelationship> CreateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default);

    Task<MetaObjectRelationship> UpdateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default);

    Task DeactivateRelationshipAsync(
        Guid relationshipUid,
        CancellationToken cancellationToken = default);

    Task TerminateRelationshipAsync(
        Guid relationshipUid,
        CancellationToken cancellationToken = default);
}