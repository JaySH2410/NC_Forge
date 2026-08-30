namespace Forge.Features.MetaSchema.Entities;

public class UuidCounter
{
    public long Id { get; set; }

    public byte EntityType { get; set; }

    public long CounterValue { get; set; }
}