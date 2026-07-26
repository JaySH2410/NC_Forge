namespace Forge.Features.Auth.DTOs.Internal;

public class RefreshTokenResult
{
    public required string RefreshToken { get; init; }

    public required string TokenHash { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}