using FluentValidation;
using Forge.Features.MetaSchema.DTOs;

namespace Forge.Features.MetaSchema.Validators;

public sealed class UpdateMetaObjectRelationshipRequestValidator
    : AbstractValidator<UpdateMetaObjectRelationshipRequest>
{
    public UpdateMetaObjectRelationshipRequestValidator()
    {
        RuleFor(x => x.Uuid)
            .NotEmpty()
            .WithMessage("Uuid is required.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(200)
            .WithMessage("Display Name cannot exceed 200 characters.")
            .When(x => x.DisplayName is not null);

        RuleFor(x => x.Description)
            .MaximumLength(4000)
            .WithMessage("Description cannot exceed 4000 characters.")
            .When(x => x.Description is not null);
    }
}
