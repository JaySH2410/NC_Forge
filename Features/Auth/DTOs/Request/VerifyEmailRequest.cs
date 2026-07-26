namespace Forge.Features.Auth.DTOs.Request;

public class VerifyEmailRequest
{
    public required string Token { get; init; }
}
