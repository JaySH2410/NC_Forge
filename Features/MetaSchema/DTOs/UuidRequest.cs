namespace Forge.Features.MetaSchema.DTOs;

public abstract class UuidRequest
{
    public required Guid Uuid { get; init; }
}