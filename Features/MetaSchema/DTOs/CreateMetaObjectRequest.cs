namespace Forge.Features.MetaSchema.DTOs;

//used for
//1. Creating a new MetaObject

public sealed class CreateMetaObjectRequest
{
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public Guid? ObjTypeUid { get; set; }
    public Guid ApplicationUid { get; set; }
}
