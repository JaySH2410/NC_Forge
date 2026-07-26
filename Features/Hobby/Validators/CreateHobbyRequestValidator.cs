using FluentValidation;
using Forge.Features.Hobby.DTOs;

namespace Forge.Features.Hobby.Validators;

public class CreateHobbyRequestValidator
    : AbstractValidator<CreateHobbyRequest>
{
    public CreateHobbyRequestValidator()
    {
        RuleFor(x => x.Name)
            .HobbyNameRules();

        RuleFor(x => x.Description)
            .HobbyDescriptionRules();
    }
}
