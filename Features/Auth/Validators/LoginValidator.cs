using FluentValidation;
using test.Features.Auth.Constants;
using test.Features.Auth.DTOs;

namespace test.Features.Auth.Validators;

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