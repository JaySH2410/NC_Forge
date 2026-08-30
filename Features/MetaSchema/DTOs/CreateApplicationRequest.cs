namespace Forge.Features.MetaSchema.DTOs;

public class CreateApplicationRequest
{
    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public required string Version { get; set; }
}
