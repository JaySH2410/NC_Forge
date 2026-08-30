using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IApplicationValidationService
{
    Task ValidateCreateApplicationAsync(Application app, CancellationToken cancellationToken);
    Task ValidateUpdateApplicationAsync(Application app, UpdateApplicationRequest request, CancellationToken cancellationToken);

}
