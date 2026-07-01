namespace test.Features.Auth.Constants;

public static class AuthErrorMessages
{
    public const string EmailAlreadyExists = "A user with this email already exists.";
    public const string InvalidCredentials = "Invalid email or password.";
    public const string Unauthorized = "Unauthorized User.";
    public const string UserNotFound = "No such User Exists.";
    public const string InvalidRefreshToken = "Invalid refresh token.";
    public const string RefreshTokenExpired = "Refresh token has expired.";
    public const string RefreshTokenRevoked = "Refresh token has already been revoked.";
    public const string InvalidPasswordResetToken = "Invalid password reset token.";
    public const string PasswordResetTokenExpired = "Password reset token has expired.";
    public const string PasswordResetTokenAlreadyUsed = "Password reset token has already been used.";
    public const string InvalidEmailVerificationToken = "Invalid email verification token.";
    public const string EmailVerificationTokenExpired = "Email verification token has expired.";
    public const string EmailVerificationTokenAlreadyUsed = "Email verification token has already been used.";
    public const string EmailAlreadyVerified = "Email address is already verified.";
}