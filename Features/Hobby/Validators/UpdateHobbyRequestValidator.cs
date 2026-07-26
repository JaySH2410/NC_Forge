using FluentValidation;
using Forge.Features.Hobby.DTOs;

namespace Forge.Features.Hobby.Validators;

public class UpdateHobbyRequestValidator
    : AbstractValidator<UpdateHobbyRequest>
{
    public UpdateHobbyRequestValidator()
    {
        RuleFor(x => x.Id)
            .HobbyIdRules();

        RuleFor(x => x.Name)
            .HobbyNameRules();

        RuleFor(x => x.Description)
            .HobbyDescriptionRules();
    }
}