namespace test.Features.Auth.DTOs.Request;

public class LogoutRequest
{
    public required string RefreshToken { get; init; }
}
