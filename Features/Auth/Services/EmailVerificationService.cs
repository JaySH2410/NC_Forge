using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Forge.Features.Auth.Constants;
using Forge.Features.Auth.Contracts;
using Forge.Features.Auth.DTOs.Internal;
using Forge.Features.Auth.Entities;
using Forge.Infrastructure.Configuration;
using Forge.Infrastructure.Persistence;
using Forge.Shared.Contracts;
using Forge.Shared.Exceptions;

namespace Forge.Features.Auth.Services;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly AppDbContext _dbContext;
    private readonly AuthenticationOptions _authOptions;
    private readonly ISecureTokenGenerator _tokenGenerator;
    public EmailVerificationService(AppDbContext dbContext, IOptions<AuthenticationOptions> options, ISecureTokenGenerator tokenGenerator)
    {
        _dbContext = dbContext;
        _authOptions = options.Value;
        _tokenGenerator = tokenGenerator;
    }

    public EmailVerificationTokenResult GenerateToken()
    {
        var token = _tokenGenerator.Generate();

        return new EmailVerificationTokenResult
        {
            Token = token,
            TokenHash = HashToken(token),
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(_authOptions.EmailVerificationTokenExpiryHours)
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

    public async Task<EmailVerificationToken> GetValidTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(token);

        var entity = await _dbContext.EmailVerificationTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (entity is null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.InvalidEmailVerificationToken);
        }

        if (entity.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.EmailVerificationTokenExpired);
        }

        if (entity.UsedAt is not null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.EmailVerificationTokenAlreadyUsed);
        }

        return entity;
    }

    public void MarkAsUsed(
        EmailVerificationToken token)
    {
        token.UsedAt = DateTimeOffset.UtcNow;
    }
}