using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaValidationService
{
    Task ValidateCreateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task ValidateUpdateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task ValidateDeleteObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task ValidateTerminateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task ValidateCreateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default);

    Task ValidateUpdateRelationshipAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task ValidateDeleteRelationshipAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task ValidateTerminateRelationshipAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);
}