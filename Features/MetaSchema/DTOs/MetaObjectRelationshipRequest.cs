namespace Forge.Features.MetaSchema.DTOs;


//used for
//1. Getting a MetaObjectRelationship by its UUID
//2. Getting a MetaObjectRelationship by its Name
//3. Creating a new MetaObjectRelationship
//4. Updating an existing MetaObjectRelationship
public sealed class MetaObjectRelationshipRequest {
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