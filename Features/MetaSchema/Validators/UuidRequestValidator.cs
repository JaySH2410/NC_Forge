using FluentValidation;
using Forge.Features.MetaSchema.DTOs;

namespace Forge.Features.MetaSchema.Validators;


public sealed class UuidRequestValidator
    : AbstractValidator<UuidRequest>
{
    public UuidRequestValidator()
    {
        RuleFor(x => x.Uuid)
            .NotEmpty()
            .WithMessage("Uuid is required.");
    }
}