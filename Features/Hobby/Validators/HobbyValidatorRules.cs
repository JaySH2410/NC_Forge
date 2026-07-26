using FluentValidation;
using Forge.Features.Hobby.Constants;

namespace Forge.Features.Hobby.Validators;

public static class HobbyValidatorRules
{
    public static IRuleBuilderOptions<T, int>
        HobbyIdRules<T>(
            this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0)
            .WithMessage(
                HobbyValidationMessages.IdGreaterThanZeroMessage);
    }

    public static IRuleBuilderOptions<T, string>
        HobbyNameRules<T>(
            this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(
                HobbyValidationMessages.NameRequiredMessage)

            .MinimumLength(HobbyConstants.NameMinLength)
            .WithMessage(
                HobbyValidationMessages.NameMinLengthMessage)

            .MaximumLength(HobbyConstants.NameMaxLength)
            .WithMessage(
                HobbyValidationMessages.NameMaxLengthMessage);
    }

    public static IRuleBuilderOptions<T, string?>
        HobbyDescriptionRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(HobbyConstants.DescriptionMaxLength)
            .WithMessage(
                HobbyValidationMessages.DescriptionMaxLengthMessage);
    }
}