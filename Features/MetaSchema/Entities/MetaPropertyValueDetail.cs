namespace Forge.Features.MetaSchema.Entities;

public sealed class MetaPropertyValueDetail
{
    public long PropertyValueId { get; set; }
    public byte[]? Value { get; set; }

    public MetaPropertyValue PropertyValue { get; set; } = null!;
}