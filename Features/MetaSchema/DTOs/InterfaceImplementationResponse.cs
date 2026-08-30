namespace Forge.Features.MetaSchema.DTOs;

public sealed class InterfaceImplementationResponse
{
    public long Id { get; set; }
    public Guid IfUid { get; set; }
    public Guid ObjUid { get; set; }
    public Guid InterfaceUid { get; set; }
    public bool IsPrimary { get; set; }
    public int Ordinal { get; set; }
    public Guid ImplementsRelationshipUid { get; set; }
    public Guid? PrimaryInterfaceRelationshipUid { get; set; }
}