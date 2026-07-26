namespace Forge.Features.Auth.DTOs.Request;

public class LogoutRequest
{
    public required string RefreshToken { get; init; }
}
