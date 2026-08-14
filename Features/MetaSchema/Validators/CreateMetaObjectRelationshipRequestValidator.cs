using FluentValidation;
using Forge.Features.MetaSchema.DTOs;

namespace Forge.Features.MetaSchema.Validators;

public sealed class CreateMetaObjectRelationshipRequestValidator
    : AbstractValidator<CreateMetaObjectRelationshipRequest>
{
    public CreateMetaObjectRelationshipRequestValidator()
    {
        RuleFor(x => x.Uuid)
            .NotEmpty()
            .WithMessage("Uuid is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(200)
            .WithMessage("Display Name cannot exceed 200 characters.")
            .When(x => x.DisplayName is not null);

        RuleFor(x => x.Description)
            .MaximumLength(4000)
            .WithMessage("Description cannot exceed 4000 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.End1Uid)
            .NotEmpty()
            .WithMessage("End1Uid is required.");

        RuleFor(x => x.End2Uid)
            .NotEmpty()
            .WithMessage("End2Uid is required.");

        RuleFor(x => x.RelTypeUid)
            .NotEmpty()
            .WithMessage("RelTypeUid is required.");
    }
}
