using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;

public interface IApplicationService
{
    Task<Application?> GetApplicationAsync(
      Guid Uid,
      CancellationToken cancellationToken = default);
    Task<Application?> GetApplicationByNameAsync(
      string Uid,
      CancellationToken cancellationToken = default);
}