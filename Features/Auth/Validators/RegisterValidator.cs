using FluentValidation;

using Forge.Features.Auth.Constants;
using Forge.Features.Auth.DTOs.Request;

namespace Forge.Features.Auth.Validators;

public class RegisterValidator
    : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
                .WithMessage(AuthValidationMessages.FirstNameRequired)
            .MinimumLength(AuthConstants.FirstNameMinLength)
                .WithMessage(AuthValidationMessages.FirstNameMinLength)
            .MaximumLength(AuthConstants.FirstNameMaxLength)
                .WithMessage(AuthValidationMessages.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
                .WithMessage(AuthValidationMessages.LastNameRequired)
            .MinimumLength(AuthConstants.LastNameMinLength)
                .WithMessage(AuthValidationMessages.LastNameMinLength)
            .MaximumLength(AuthConstants.LastNameMaxLength)
                .WithMessage(AuthValidationMessages.LastNameMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage(AuthValidationMessages.EmailRequired)
            .EmailAddress()
                .WithMessage(AuthValidationMessages.EmailInvalid)
            .MaximumLength(AuthConstants.EmailMaxLength)
                .WithMessage(AuthValidationMessages.EmailMaxLength);

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage(AuthValidationMessages.PasswordRequired)
            .MinimumLength(AuthConstants.PasswordMinLength)
                .WithMessage(AuthValidationMessages.PasswordMinLength)
            .MaximumLength(AuthConstants.PasswordMaxLength)
                .WithMessage(AuthValidationMessages.PasswordMaxLength)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$")
                .WithMessage(AuthValidationMessages.PasswordComplexity);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
                .WithMessage(AuthValidationMessages.ConfirmPasswordRequired)
            .Equal(x => x.Password)
                .WithMessage(AuthValidationMessages.PasswordMismatch);
    }
}