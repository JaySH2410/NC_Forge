
using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;
using Forge.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Forge.Features.MetaSchema.Services;

//This service is responsible for creation, updation, deletion of MetaScheme (Objects and Relationships)
// only creation for now
//It will check for the following:
//1. CreateObjectAsync
//2. UpdateObjectAsync
//3. DeactivateObjectAsync
//4. TerminateObjectAsync
//5. CreateRelationshipAsync
//6. UpdateRelationshipAsync
//7. DeactivateRelationshipAsync
//8. TerminateRelationshipAsync

public class MetaSchemaAuthoringService: IMetaSchemaAuthoringService
{
    private readonly AppDbContext _context;

    private readonly IMetaSchemaValidationService _validationService;

    public MetaSchemaAuthoringService(
        AppDbContext context,
        IMetaSchemaValidationService validationService)
    {
        _context = context;
        _validationService = validationService;
    }

    public async Task<MetaObject> CreateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateCreateObjectAsync(
            metaObject,
            cancellationToken);

        _context.MetaObjects.Add(metaObject);

        await _context.SaveChangesAsync(cancellationToken);

        return metaObject;
    }

    public async Task<MetaObject> UpdateObjectAsync(
        UpdateMetaObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        var existingObject = await _context.MetaObjects
            .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
            cancellationToken);

        if (existingObject is null)
            throw new NotFoundException($"Object with '{request.Uuid}' | '{request.DisplayName}' was not found");

        await _validationService.ValidateUpdateObjectAsync(
            existingObject,
            request,
            cancellationToken);

        existingObject.DisplayName = request.DisplayName;
        existingObject.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return existingObject;
    }

    public async Task DeactivateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default){
        var existingObject = await _context.MetaObjects
            .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
            cancellationToken);

        if (existingObject is null)
            throw new NotFoundException($"Object with '{request.Uuid}' was not found");

        await _validationService.ValidateDeactivateObjectAsync(
                existingObject,
                cancellationToken);

        existingObject.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActivateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default)
    {
        var existingObject = await _context.MetaObjects
            .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
            cancellationToken);

        if (existingObject is null)
            throw new NotFoundException($"Object with '{request.Uuid}' was not found");

        await _validationService.ValidateActivateObjectAsync(
                existingObject,
                cancellationToken);

        existingObject.IsActive = true;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task TerminateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //Load Object
        //      │
        //      ▼
        //Validate Terminate
        //      │
        //      ▼
        //Load All Relationships
        //(End1 = Object OR End2 = Object)
        //      │
        //      ▼
        //Delete Relationships
        //      │
        //      ▼
        //Delete Object
        //      │
        //      ▼
        //SaveChanges()
    }

    public async Task<MetaObjectRelationship> CreateRelationshipAsync(
        MetaObjectRelationship request,
        CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateCreateRelationshipAsync(
            request,
            cancellationToken);

        _context.MetaObjectRelationships.Add(request);

        await _context.SaveChangesAsync(cancellationToken);

        return request;
    }

    public async Task<MetaObjectRelationship> UpdateRelationshipAsync(
        UpdateMetaObjectRelationshipRequest request,
        CancellationToken cancellationToken = default){
        var existingRel = await _context.MetaObjectRelationships
            .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
            cancellationToken);

        if (existingRel is null)
            throw new NotFoundException($"Relationship with '{request.Uuid}' | '{request.DisplayName}' was not found");

        await _validationService.ValidateUpdateRelationshipAsync(
            existingRel,
            request,
            cancellationToken);

        existingRel.DisplayName = request.DisplayName;
        existingRel.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return existingRel;
    }
    
    public async Task DeactivateRelationshipAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default){
        var existingRel = await _context.MetaObjectRelationships
            .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
            cancellationToken);

        if (existingRel is null)
            throw new NotFoundException($"Object with '{request.Uuid}' was not found");

        await _validationService.ValidateDeactivateRelationshipAsync(
                existingRel,
                cancellationToken);

        existingRel.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActivateRelationshipAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default)
    {
            var existingRel = await _context.MetaObjectRelationships
                .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
                cancellationToken);

            if (existingRel is null)
                throw new NotFoundException($"Object with '{request.Uuid}' was not found");

            await _validationService.ValidateActivateRelationshipAsync(
                    existingRel,
                    cancellationToken);

            existingRel.IsActive = true;

            await _context.SaveChangesAsync(cancellationToken);
        }

    public async Task TerminateRelationshipAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default){
        throw new NotImplementedException();
        //Load Relationship
        //      │
        //      ▼
        //Validate Terminate
        //      │
        //      ▼
        //Delete Relationship
        //      │
        //      ▼
        //SaveChanges()
    }
}