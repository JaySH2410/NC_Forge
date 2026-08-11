using FluentValidation;
using Forge.Features.MetaSchema.DTOs;

namespace Forge.Features.MetaSchema.Validators;


public sealed class CreateMetaObjectRequestValidator
    : AbstractValidator<CreateMetaObjectRequest>
{
    public CreateMetaObjectRequestValidator()
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

        RuleFor(x => x.ApplicationUid)
            .NotEmpty()
            .WithMessage("ApplicationUid is required.");

        RuleFor(x => x.Version)
            .NotEmpty()
            .WithMessage("Version is required.")
            .MaximumLength(40)
            .WithMessage("Version cannot exceed 40 characters.");
    }
}