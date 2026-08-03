using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaAuthoringService
{
    Task<MetaObject> CreateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task<MetaObject> UpdateObjectAsync(
        UpdateMetaObjectRequest request,
        CancellationToken cancellationToken = default);

    Task DeactivateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);

    Task TerminateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);

    Task<MetaObjectRelationship> CreateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default);

    Task<MetaObjectRelationship> UpdateRelationshipAsync(
        UpdateMetaObjectRelationshipRequest relationship,
        CancellationToken cancellationToken = default);

    Task DeactivateRelationshipAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);

    Task ActivateRelationshipAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);

    Task TerminateRelationshipAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);
}