using Forge.Features.Auth.DTOs.Internal;

namespace Forge.Features.Auth.DTOs.Response;

public class RefreshTokenResponse
{
    public required TokenResult Tokens { get; init; }
}
