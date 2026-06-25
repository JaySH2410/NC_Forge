using FluentValidation;
using test.Features.Hobby.DTOs;

namespace test.Features.Hobby.Validators;

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