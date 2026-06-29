using test.Features.Auth.DTOs;
using test.Features.Auth.Entities;

namespace test.Features.Auth.Contracts;

public interface IRefreshTokenService
{
    RefreshTokenResult GenerateRefreshToken();

    string HashToken(string token);

    bool VerifyToken(
        string token,
        string tokenHash);

    Task<RefreshToken> GetValidRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    void RevokeRefreshToken(
        RefreshToken refreshToken,
        string reason);

    Task RevokeAllUserRefreshTokensAsync(
    int userId,
        string reason,
        CancellationToken cancellationToken = default);
}