namespace Forge.Features.MetaSchema.DTOs;

//used for
//1. Getting a MetaObject by its UUID
//2. Getting a MetaObject by its Name
//3. Creating a new MetaObject
//4. Updating an existing MetaObject
public sealed class MetaObjectResponse
{
    public int Id { get; set; }
    public Guid Uuid { get; set; }
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public Guid? ObjTypeUid { get; set; }
    public Guid ApplicationUid { get; set; }
    public string Version { get; set; } = null!;
}