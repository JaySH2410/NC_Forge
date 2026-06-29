namespace test.Features.Auth.Constants;

public static class AuthErrorMessages
{
    public const string EmailAlreadyExists =
        "A user with this email already exists.";

    public const string InvalidCredentials =
        "Invalid email or password.";

    public const string Unauthorized =
        "Unauthorized User.";

    public const string UserNotFound =
        "No such User Exists.";

    public const string InvalidRefreshToken =
     "Invalid refresh token.";

    public const string RefreshTokenExpired =
        "Refresh token has expired.";

    public const string RefreshTokenRevoked =
        "Refresh token has already been revoked.";
}