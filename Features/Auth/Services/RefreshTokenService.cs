using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using test.Features.Auth.Contracts;
using test.Features.Auth.DTOs;
using test.Infrastructure.Configuration;

namespace test.Features.Auth.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenService(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }
    public RefreshTokenResult GenerateRefreshToken()
    {
        var token = GenerateToken();

        return new RefreshTokenResult
        {
            RefreshToken = token,
            TokenHash = HashToken(token),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays)
        };
    }
    private string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }

    public string HashToken(
        string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }

    public bool VerifyToken(
        string token,
        string tokenHash)
    {
        var hash = HashToken(token);

        return hash == tokenHash;
    }
}