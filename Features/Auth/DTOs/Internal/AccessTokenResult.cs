namespace Forge.Features.Auth.DTOs.Internal;

public sealed class AccessTokenResult
{
    public required string AccessToken { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }

}