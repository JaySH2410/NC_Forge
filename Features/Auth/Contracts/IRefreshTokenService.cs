using Forge.Features.Auth.DTOs.Internal;
using Forge.Features.Auth.Entities;

namespace Forge.Features.Auth.Contracts;

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