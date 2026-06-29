using test.Features.Auth.DTOs;

namespace test.Features.Auth.Contracts;

public interface IRefreshTokenService
{
    RefreshTokenResult GenerateRefreshToken();

    string HashToken(string token);

    bool VerifyToken(
        string token,
        string tokenHash);
}