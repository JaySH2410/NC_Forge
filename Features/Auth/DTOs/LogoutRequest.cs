namespace test.Features.Auth.DTOs;

public class LogoutRequest
{
    public required string RefreshToken { get; init; }
}
