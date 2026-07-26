using FluentValidation;
using Forge.Features.Auth.Constants;
using Forge.Features.Auth.DTOs.Request;

namespace Forge.Features.Auth.Validators;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage(AuthValidationMessages.EmailRequired)
            .EmailAddress()
                .WithMessage(AuthValidationMessages.EmailInvalid)
            .MaximumLength(AuthConstants.EmailMaxLength)
                .WithMessage(AuthValidationMessages.EmailMaxLength);

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage(AuthValidationMessages.PasswordRequired);
    }
}