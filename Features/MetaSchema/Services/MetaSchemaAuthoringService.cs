
using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;
using Forge.Shared.Exceptions;
using Forge.Shared.Identifiers;
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
    private readonly IMetaSchemaService _metaSchemaService;
    private readonly IForgeUuidGenerator _uuidGenerator;

    public MetaSchemaAuthoringService(
        AppDbContext context,
        IMetaSchemaValidationService validationService,
        IMetaSchemaService metaSchemaService,
        IForgeUuidGenerator uuidGenerator
        )
    {
        _context = context;
        _validationService = validationService;
        _metaSchemaService = metaSchemaService;
        _uuidGenerator = uuidGenerator;
    }

    public async Task<MetaObjectResponse> CreateObjectAsync(
        CreateMetaObjectRequest metaObject,
        CancellationToken cancellationToken = default)
    {
        MetaObject metaObjectEntity = new MetaObject
        {
            //Uuid = await _uuidGenerator.GenerateMetaObjectUuidAsync(cancellationToken),
            Name = metaObject.Name,
            DisplayName = metaObject.DisplayName,
            Description = metaObject.Description,
            ObjTypeUid = metaObject.ObjTypeUid,
            ApplicationUid = metaObject.ApplicationUid,
            Version = metaObject.Version
        };
        await _validationService.ValidateCreateObjectAsync(
            metaObjectEntity,
            cancellationToken);

        metaObjectEntity.Uuid = await _uuidGenerator.GenerateMetaObjectUuidAsync(cancellationToken);

        _context.MetaObjects.Add(metaObjectEntity);

        await _context.SaveChangesAsync(cancellationToken);

        MetaObjectResponse response = new MetaObjectResponse
        {
            Id = metaObjectEntity.Id,
            Uuid = metaObjectEntity.Uuid,
            Name = metaObjectEntity.Name,
            DisplayName = metaObjectEntity.DisplayName,
            Description = metaObjectEntity.Description,
            ObjTypeUid = metaObjectEntity.ObjTypeUid,
            ApplicationUid = metaObjectEntity.ApplicationUid,
            Version = metaObjectEntity.Version
        };

        return response;
    }

    public async Task<MetaObjectResponse> UpdateObjectAsync(
        UpdateMetaObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        var existingObject = await _metaSchemaService.GetObjectAsync(request.Uuid, cancellationToken);

        if (existingObject == null) {
            throw new NotFoundException($"Object with '{request.Uuid}' was not found");
        }

        await _validationService.ValidateUpdateObjectAsync(
            existingObject,
            request,
            cancellationToken);

        existingObject.DisplayName = request.DisplayName;
        existingObject.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new MetaObjectResponse
        {
            Id = existingObject.Id,
            Uuid = existingObject.Uuid,
            Name = existingObject.Name,
            DisplayName = existingObject.DisplayName,
            Description = existingObject.Description,
            ObjTypeUid = existingObject.ObjTypeUid,
            ApplicationUid = existingObject.ApplicationUid,
            Version = existingObject.Version
        };
        return response;
    }

    public async Task DeactivateObjectAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default){
        var existingObject = await _metaSchemaService.GetObjectAsync(request.Uuid, cancellationToken);

        if (existingObject == null)
        {
            throw new NotFoundException($"Object with '{request.Uuid}' was not found");
        }
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
        var existingObject = await _metaSchemaService.GetObjectAsync(request.Uuid, cancellationToken);

        if (existingObject == null)
        {
            throw new NotFoundException($"Object with '{request.Uuid}' was not found");
        }
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

    //private async Task<MetaObject> GetObjectOrThrowAsync(UuidRequest request,CancellationToken cancellationToken)
    //{
    //    var metaObject = await _context.MetaObjects
    //        .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
    //        cancellationToken);

    //    if(metaObject is null)
    //        throw new NotFoundException($"Object with '{request.Uuid}' was not found");

    //    return metaObject;
    //}

    public async Task<MetaObjectRelationshipResponse> CreateRelationshipAsync(
        CreateMetaObjectRelationshipRequest request,
        CancellationToken cancellationToken = default)
    {
        MetaObjectRelationship requestEntity = new MetaObjectRelationship
        {
            //Uuid = await _uuidGenerator.GenerateRelationshipUuidAsync(cancellationToken),
            Name = request.Name,
            DisplayName = request.DisplayName,
            Description = request.Description,
            End1Uid = request.End1Uid,
            End2Uid = request.End2Uid,
            RelTypeUid = request.RelTypeUid,
            Ordinal = request.Ordinal
        };
        await _validationService.ValidateCreateRelationshipAsync(
            requestEntity,
            cancellationToken);

        requestEntity.Uuid = await _uuidGenerator.GenerateRelationshipUuidAsync(cancellationToken);
        
        _context.MetaObjectRelationships.Add(requestEntity);

        await _context.SaveChangesAsync(cancellationToken);

        var response = new MetaObjectRelationshipResponse
        {
            Id = requestEntity.Id,
            Uuid = requestEntity.Uuid,
            Name = requestEntity.Name,
            DisplayName = requestEntity.DisplayName,
            Description = requestEntity.Description,
            End1Uid = requestEntity.End1Uid,
            End2Uid = requestEntity.End2Uid,
            RelTypeUid = requestEntity.RelTypeUid,
            Ordinal = requestEntity.Ordinal
        };

        return response;
    }

    public async Task<MetaObjectRelationshipResponse> UpdateRelationshipAsync(
        UpdateMetaObjectRelationshipRequest request,
        CancellationToken cancellationToken = default)
    {
        var existingRel = await _metaSchemaService.GetRelationshipAsync(request.Uuid, cancellationToken);

        if (existingRel == null)
            throw new NotFoundException($"Relationship with '{request.Uuid}' was not found");


        await _validationService.ValidateUpdateRelationshipAsync(
        existingRel,
        request,
        cancellationToken);

        existingRel.DisplayName = request.DisplayName;
        existingRel.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new MetaObjectRelationshipResponse
        {
            Id = existingRel.Id,
            Uuid = existingRel.Uuid,
            Name = existingRel.Name,
            DisplayName = existingRel.DisplayName,
            Description = existingRel.Description,
            End1Uid = existingRel.End1Uid,
            End2Uid = existingRel.End2Uid,
            RelTypeUid = existingRel.RelTypeUid,
            Ordinal = existingRel.Ordinal
        };

        return response;
    }

    public async Task DeactivateRelationshipAsync(
        UuidRequest request,
        CancellationToken cancellationToken = default){
        var existingRel = await _metaSchemaService.GetRelationshipAsync(request.Uuid, cancellationToken);

        if (existingRel == null)
            throw new NotFoundException($"Relationship with '{request.Uuid}' was not found");

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
        var existingRel = await _metaSchemaService.GetRelationshipAsync(request.Uuid, cancellationToken);

        if (existingRel == null)
            throw new NotFoundException($"Relationship with '{request.Uuid}' was not found");


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

    //private async Task<MetaObjectRelationship> GetRelationshipOrThrowAsync(UuidRequest request, CancellationToken cancellationToken)
    //{
    //    var metaRelObject = await _context.MetaObjectRelationships
    //        .FirstOrDefaultAsync(x => x.Uuid == request.Uuid,
    //        cancellationToken);

    //    if (metaRelObject is null)
    //        throw new NotFoundException($"Relationship with '{request.Uuid}' was not found");

    //    return metaRelObject;
    //}

}