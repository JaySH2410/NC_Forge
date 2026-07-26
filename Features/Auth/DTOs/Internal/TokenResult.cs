namespace Forge.Features.Auth.DTOs.Internal;

public class TokenResult
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }

    public DateTimeOffset AccessTokenExpiresAt { get; init; }

    public DateTimeOffset RefreshTokenExpiresAt { get; init; }
}
