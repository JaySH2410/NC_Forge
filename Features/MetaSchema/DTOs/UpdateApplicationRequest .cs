using Forge.Features.MetaSchema.Versioning;


namespace Forge.Features.MetaSchema.DTOs;

public class UpdateApplicationRequest: UuidRequest
{
    public string? DisplayName { get; set; }

    public string? Description { get; init; }

    public required ApplicationVersionIncrement VersionIncrement { get; init; }
}
