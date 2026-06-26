namespace test.Features.Auth.DTOs;

public class LoginResponse
{
    public required string AccessToken { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}
