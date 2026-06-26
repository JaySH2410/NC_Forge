namespace test.Features.Auth.DTOs;

public sealed class TokenResult
{
    public required string AccessToken { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}