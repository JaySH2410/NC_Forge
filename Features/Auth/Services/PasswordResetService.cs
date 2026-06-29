using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using test.Features.Auth.Constants;
using test.Features.Auth.Contracts;
using test.Features.Auth.DTOs;
using test.Features.Auth.Entities;
using test.Infrastructure.Configuration;
using test.Infrastructure.Persistence;
using test.Shared.Exceptions;

namespace test.Features.Auth.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;

    public PasswordResetService(
        AppDbContext dbContext,
        IOptions<JwtOptions> options)
    {
        _dbContext = dbContext;
        _jwtOptions = options.Value;
    }

    public PasswordResetTokenResult GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        var token = Convert.ToBase64String(bytes);

        return new PasswordResetTokenResult
        {
            Token = token,
            TokenHash = HashToken(token),
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.PasswordResetTokenExpiryMinutes)
        };
    }

    public string HashToken(
        string token)
    {
        var bytes = SHA256.HashData(
            Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }

    public bool VerifyToken(
        string token,
        string tokenHash)
    {
        return HashToken(token) == tokenHash;
    }

    public async Task<PasswordResetToken> GetValidTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var hash = HashToken(token);

        var entity = await _dbContext.PasswordResetTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.TokenHash == hash,
                cancellationToken);

        if (entity is null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.InvalidPasswordResetToken);
        }

        if (entity.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.PasswordResetTokenExpired);
        }

        if (entity.UsedAt is not null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.PasswordResetTokenAlreadyUsed);
        }

        return entity;
    }

    public void MarkAsUsed(
        PasswordResetToken token)
    {
        token.UsedAt = DateTimeOffset.UtcNow;
    }
}