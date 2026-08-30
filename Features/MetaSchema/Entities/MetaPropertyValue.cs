using Forge.Shared.Entities;

namespace Forge.Features.MetaSchema.Entities;

public class MetaPropertyValue: ActivatableEntity
{
    public Guid ObjUid { get; set; }
    
    public Guid IfUid { get; set; }

    public Guid PropertyUid { get; set;  } 
    
    // public int Ordinal { get; set; }

    public string? ValueString { get; set; }
    
    public double? ValueFloat { get; set; }

    public bool? ValueBool { get; set; }
    
    public DateTimeOffset? ValueDateTime { get; set; }
    
    public string? Uom { get; set; }

    public bool IsExtended { get; set; }
    public MetaPropertyValueDetail? Detail { get; set; }
}   