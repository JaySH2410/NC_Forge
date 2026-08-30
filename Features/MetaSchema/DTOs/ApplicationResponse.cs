namespace Forge.Features.MetaSchema.DTOs;

public sealed class ApplicationResponse
{
    public long Id { get; set; }

    public Guid Uuid { get; set; }

    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public required string Version { get; set; }
}
