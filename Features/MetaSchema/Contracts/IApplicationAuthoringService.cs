using Forge.Features.MetaSchema.DTOs;

namespace Forge.Features.MetaSchema.Contracts;

public interface IApplicationAuthoringService
{
    Task<ApplicationResponse> CreateApplicationAsync(
       CreateApplicationRequest request,
       CancellationToken cancellationToken = default);
    Task<ApplicationResponse> UpdateApplicationAsync(
        UpdateApplicationRequest request,
        CancellationToken cancellationToken = default);
}
