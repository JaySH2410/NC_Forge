using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Forge.Features.MetaSchema.Services;

public class ApplicationService: IApplicationService
{
    private readonly AppDbContext _context;

    public ApplicationService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Application?> GetApplicationAsync(
       Guid Uid,
       CancellationToken cancellationToken = default)
    {
        return await _context.Applications
            //.AsNoTracking()
            .FirstOrDefaultAsync(
            x => x.Uuid == Uid,
            cancellationToken);
    }
    public async Task<Application?> GetApplicationByNameAsync(
      string name,
      CancellationToken cancellationToken = default)
    {
        return await _context.Applications
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name == name,
                cancellationToken);
    }
}
