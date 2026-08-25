using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;
using Forge.Shared.Exceptions;
using Forge.Shared.Identifiers;

namespace Forge.Features.MetaSchema.Services;

public class ApplicationAuthoringService: IApplicationAuthoringService
{
    private readonly AppDbContext _context;
    private readonly IForgeUuidGenerator _uuidGenerator;
    private readonly IApplicationValidationService _validationService;
    private readonly IApplicationService _app;

    public ApplicationAuthoringService(
        IForgeUuidGenerator uuidGenerator,
        AppDbContext context,
        IApplicationValidationService validationService,
        IApplicationService app)
    {
        _uuidGenerator = uuidGenerator;
        _context = context;
        _validationService = validationService;
        _app = app;
    }
    public async Task<ApplicationResponse> CreateApplicationAsync(
       CreateApplicationRequest request,
       CancellationToken cancellationToken = default)
    {
        Application requestEntity = new Application
        {
            Name = request.Name,
            DisplayName = request.DisplayName,
            Description = request.Description,
            Version = request.Version
        };

        await _validationService.ValidateCreateApplicationAsync(requestEntity, cancellationToken);

        requestEntity.Uuid = await _uuidGenerator.GenerateApplicationUuidAsync(cancellationToken);

        _context.Applications.Add(requestEntity);

        await _context.SaveChangesAsync(cancellationToken);

        ApplicationResponse response = new ApplicationResponse
        {
            Id = requestEntity.Id,
            Uuid = requestEntity.Uuid,
            Name = requestEntity.Name,
            DisplayName = requestEntity.DisplayName,
            Description = requestEntity.Description,
            Version = requestEntity.Version
        };

        return response;
    }

    public async Task<ApplicationResponse> UpdateApplicationAsync(
       UpdateApplicationRequest request,
       CancellationToken cancellationToken = default)
    {
        var existingObject = await _app.GetApplicationAsync(request.Uuid, cancellationToken);

        if (existingObject == null)
        {
            throw new NotFoundException($"Object with '{request.Uuid}' was not found");
        }

        await _validationService.ValidateUpdateApplicationAsync(existingObject, request,cancellationToken);

        existingObject.DisplayName = request.DisplayName;
        existingObject.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new ApplicationResponse
        {
            Id = existingObject.Id,
            Uuid = existingObject.Uuid,
            Name = existingObject.Name,
            DisplayName = existingObject.DisplayName,
            Description = existingObject.Description,
            Version = existingObject.Version
        };

        return response;
    }
}
