namespace Forge.Features.MetaSchema.DTOs;

public sealed class CreateInterfaceImplementationRequest
{
    public Guid ObjUid { get; set; }
    public Guid InterfaceUid { get; set; }
    public bool IsPrimary { get; set; }
    public int Ordinal { get; set; }
}