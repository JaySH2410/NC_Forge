using Forge.Features.MetaSchema.Versioning;

namespace Forge.Features.MetaSchema.DTOs;

public sealed class UpdateMetaObjectRequest: UuidRequest
{
    public string? DisplayName { get; set; }

    public string? Description { get; init; }

    public required ObjectVersionIncrement VersionIncrement { get; init; }
}
