using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IMetaSchemaValidationService
{
    Task ValidateCreateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default);

    Task ValidateCreateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default);
}