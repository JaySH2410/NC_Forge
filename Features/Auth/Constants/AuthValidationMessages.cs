namespace test.Features.Auth.Constants;

public class AuthValidationMessages
{
    public const string FirstNameRequired = "First name is required.";
    public const string FirstNameMinLength = "First name must be at least 3 characters.";
    public const string FirstNameMaxLength = "First name cannot exceed 100 characters.";
    public const string LastNameRequired = "Last name is required.";
    public const string LastNameMinLength = "Last name must be at least 3 characters.";
    public const string LastNameMaxLength = "Last name cannot exceed 100 characters.";
    public const string EmailRequired = "Email is required.";
    public const string EmailInvalid = "Email format is invalid.";
    public const string EmailMaxLength = "Email cannot exceed 255 characters.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordMinLength = "Password must be at least 8 characters.";
    public const string PasswordMaxLength = "Password cannot exceed 128 characters.";
    public const string PasswordComplexity = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
    public const string ConfirmPasswordRequired = "Confirm password is required.";
    public const string PasswordMismatch = "Password and confirm password do not match.";
}
