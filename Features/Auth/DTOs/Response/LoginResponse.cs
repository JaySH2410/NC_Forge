using test.Features.Auth.DTOs.Internal;

namespace test.Features.Auth.DTOs.Response;

public class LoginResponse
{
    public required TokenResult Tokens { get; init; }
}
