namespace Forge.Features.MetaSchema.DTOs;

public sealed class UpdateMetaObjectRelationshipRequest : UuidRequest
{
    public string? DisplayName { get; set; }

    public string? Description { get; init; }
}