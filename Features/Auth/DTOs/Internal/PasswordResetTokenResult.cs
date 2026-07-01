namespace test.Features.Auth.DTOs.Internal;

public class PasswordResetTokenResult
{
    public required string Token { get; init; }

    public required string TokenHash { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}
