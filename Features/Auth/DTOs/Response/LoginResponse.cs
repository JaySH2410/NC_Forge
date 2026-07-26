using Forge.Features.Auth.DTOs.Internal;

namespace Forge.Features.Auth.DTOs.Response;

public class LoginResponse
{
    public required TokenResult Tokens { get; init; }
}
