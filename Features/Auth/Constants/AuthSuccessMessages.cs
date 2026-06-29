using System.Globalization;

namespace test.Features.Auth.Constants;

public static class AuthSuccessMessages
{
    public const string RegisterSuccess = "User Registered Successfully.";
    public const string LoginSuccess = "User Loged In Successfully.";
    public const string CurrentUserFetched = "Successfully Fetched Current User Information.";
    public const string TokenRefreshed = "Token Refreshed Successfully.";
    public const string LogoutSuccess = "User Loged Out Successfully.";
    public const string PasswordChangedSuccessfully = "Password Changed Successfully.";
    public const string PasswordResetEmailSent = "If an account exists for the provided email address, a password reset link has been sent.";
    public const string PasswordResetSuccessful = "Password has been reset successfully.";
}