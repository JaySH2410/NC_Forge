using FluentValidation;
using Forge.Features.MetaSchema.DTOs;

namespace Forge.Features.MetaSchema.Validators;

public sealed class CreateInterfaceImplementationRequestValidator
    : AbstractValidator<CreateInterfaceImplementationRequest>
{
    public CreateInterfaceImplementationRequestValidator()
    {
        RuleFor(x => x.ObjUid).NotEmpty();
        RuleFor(x => x.InterfaceUid).NotEmpty();
        RuleFor(x => x.Ordinal).GreaterThanOrEqualTo(0);
    }
}