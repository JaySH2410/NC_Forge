using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;
using Forge.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace Forge.Features.MetaSchema.Services;

public class ApplicationValidationService: IApplicationValidationService
{
    private readonly AppDbContext _context;

    public ApplicationValidationService(AppDbContext context)
    {
        _context = context;
    }
    public async Task ValidateCreateApplicationAsync(Application newApp, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(newApp.Name))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "Name", ["Name is required."] } });
        }
        //Checking the DisplayName is not null or empty
        if (string.IsNullOrWhiteSpace(newApp.DisplayName))
        {
            newApp.DisplayName = newApp.Name;
        }

        Application? existingObject = await _context.Applications
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name == newApp.Name,
                cancellationToken);

        if (existingObject is not null)
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "Name", [$"Application with name '{newApp.Name}' already exists."] }
                });
        }
    }
    public async Task ValidateUpdateApplicationAsync(Application existingApp, UpdateApplicationRequest request,CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            request.DisplayName = existingApp.Name;
        }
        await Task.CompletedTask;
    }

}
