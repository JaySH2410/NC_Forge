using FluentValidation;
using test.Features.Auth.DTOs;

namespace test.Features.Auth.Validators;

public class ForgotPasswordValidator
    : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}