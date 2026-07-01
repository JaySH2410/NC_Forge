using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using test.Features.Auth.Constants;
using test.Features.Auth.Contracts;
using test.Features.Auth.DTOs.Internal;
using test.Features.Auth.Entities;
using test.Infrastructure.Configuration;
using test.Infrastructure.Persistence;
using test.Shared.Contracts;
using test.Shared.Exceptions;

namespace test.Features.Auth.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _dbContext;
    private readonly AuthenticationOptions _authOptions;
    private readonly ISecureTokenGenerator _tokenGenerator;

    public RefreshTokenService(AppDbContext dbContext, IOptions<AuthenticationOptions> options, ISecureTokenGenerator tokenGenerator)
    {
        _dbContext = dbContext;
        _authOptions = options.Value;
        _tokenGenerator = tokenGenerator;
    }
    public RefreshTokenResult GenerateRefreshToken()
    {
        var token = _tokenGenerator.Generate();

        return new RefreshTokenResult
        {
            RefreshToken = token,
            TokenHash = HashToken(token),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_authOptions.RefreshTokenExpiryDays)
        };
    }
    public string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
    public bool VerifyToken(string token,string tokenHash)
    {
        var hash = HashToken(token);

        return hash == tokenHash;
    }
    public async Task<RefreshToken> GetValidRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(refreshToken);

        var entity = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (entity is null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.InvalidRefreshToken);
        }

        if (entity.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.RefreshTokenExpired);
        }

        if (entity.RevokedAt is not null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.RefreshTokenRevoked);
        }

        return entity;
    }
    public void RevokeRefreshToken(RefreshToken refreshToken, string reason)
    {
        refreshToken.RevokedAt = DateTimeOffset.UtcNow;
        refreshToken.ReplacedByTokenHash = null;
        refreshToken.RevokedReason = reason;
    }
    public async Task RevokeAllUserRefreshTokensAsync(int userId,string reason,CancellationToken cancellationToken = default)
    {
        var refreshTokens = await _dbContext.RefreshTokens
            .Where(x =>
                x.UserId == userId &&
                x.RevokedAt == null &&
                x.ExpiresAt > DateTimeOffset.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in refreshTokens)
        {
            RevokeRefreshToken(
                token,
                reason);
        }
    }
    //private string GenerateToken()
    //{
    //    var bytes = RandomNumberGenerator.GetBytes(64);

    //    return Convert.ToBase64String(bytes);
    //}
}