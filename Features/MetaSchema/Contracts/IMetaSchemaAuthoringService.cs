using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaAuthoringService
{
    Task<MetaObjectResponse> CreateObjectAsync(
        CreateMetaObjectRequest metaObject,
        CancellationToken cancellationToken = default);

    Task<MetaObjectResponse> UpdateObjectAsync(
        UpdateMetaObjectRequest request,
        CancellationToken cancellationToken = default);

    Task ActivateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);

    Task DeactivateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);

    Task TerminateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default);

    Task<MetaObjectRelationshipResponse> CreateRelationshipAsync(
        CreateMetaObjectRelationshipRequest relationship,
        CancellationToken cancellationToken = default);

    Task<MetaObjectRelationshipResponse> UpdateRelationshipAsync(
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