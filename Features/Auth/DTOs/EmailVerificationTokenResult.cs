namespace test.Features.Auth.DTOs;

public class EmailVerificationTokenResult
{
    public required string Token { get; init; }

    public required string TokenHash { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}