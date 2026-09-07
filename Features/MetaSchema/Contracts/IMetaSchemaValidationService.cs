using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaValidationService
{
    Task ValidateCreateObjectAsync(
        MetaObject metaObject,
        Application? application,
        CancellationToken cancellationToken = default);

    Task ValidateUpdateObjectAsync(
        MetaObject metaObject,
        UpdateMetaObjectRequest request,
        Application? application,
        CancellationToken cancellationToken = default);

    Task ValidateDeactivateObjectAsync(
        MetaObject existingObject,
        CancellationToken cancellationToken = default);

    Task ValidateActivateObjectAsync(
        MetaObject existingObject,
        CancellationToken cancellationToken = default);

    Task ValidateTerminateObjectAsync(
        MetaObject existingObject,
        CancellationToken cancellationToken = default);

    Task ValidateCreateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default);

    void ValidateGenericRelationshipAuthoringAllowed(
        Guid relationshipTypeUid);

    Task ValidateCreateInterfaceImplementationAsync(
        CreateInterfaceImplementationRequest request,
        CancellationToken cancellationToken = default);

    Task ValidateUpdateRelationshipAsync(
        MetaObjectRelationship existingRel,
        UpdateMetaObjectRelationshipRequest request,
        CancellationToken cancellationToken = default);

    Task ValidateDeactivateRelationshipAsync(
        MetaObjectRelationship existingRel,
        CancellationToken cancellationToken = default);

    Task ValidateActivateRelationshipAsync(
        MetaObjectRelationship existingRel,
        CancellationToken cancellationToken = default);

    Task ValidateTerminateRelationshipAsync(
        MetaObjectRelationship existingRel,
        CancellationToken cancellationToken = default);
}
