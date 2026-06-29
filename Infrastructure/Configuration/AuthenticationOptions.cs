namespace test.Infrastructure.Configuration;

public class AuthenticationOptions
{
    public const string SectionName = "Authentication";

    public int AccessTokenExpiryMinutes { get; init; }

    public int RefreshTokenExpiryDays { get; init; }

    public int PasswordResetTokenExpiryMinutes { get; init; }

    public int EmailVerificationTokenExpiryHours { get; init; }
}