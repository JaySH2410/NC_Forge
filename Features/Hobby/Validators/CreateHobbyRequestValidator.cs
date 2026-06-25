using FluentValidation;
using test.Features.Hobby.DTOs;

namespace test.Features.Hobby.Validators;

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
