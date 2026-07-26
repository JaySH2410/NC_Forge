using FluentValidation;
using Forge.Features.Auth.DTOs.Request;

namespace Forge.Features.Auth.Validators;

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