namespace test.Infrastructure.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public required string Secret { get; init; }

    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public int AccessTokenExpiryMinutes { get; init; }

    public int RefreshTokenExpiryDays { get; init; }

    public int PasswordResetTokenExpiryMinutes { get; init; }
}