using test.Features.Auth.DTOs.Internal;

namespace test.Features.Auth.DTOs.Response;

public class RefreshTokenResponse
{
    public required TokenResult Tokens { get; init; }
}
