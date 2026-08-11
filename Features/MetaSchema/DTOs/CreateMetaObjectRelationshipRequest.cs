namespace Forge.Features.MetaSchema.DTOs;


//used for
//1. Creating a new MetaObjectRelationship
public sealed class CreateMetaObjectRelationshipRequest {
    public int Id { get; set; }
    public Guid Uuid { get; set; }
    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public Guid End1Uid { get; set; }

    public Guid End2Uid { get; set; }

    public Guid RelTypeUid { get; set; }

    public int Ordinal { get; set; }
}