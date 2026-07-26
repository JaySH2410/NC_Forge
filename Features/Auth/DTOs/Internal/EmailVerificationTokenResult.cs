namespace Forge.Features.Auth.DTOs.Internal;

public class EmailVerificationTokenResult
{
    public required string Token { get; init; }

    public required string TokenHash { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}