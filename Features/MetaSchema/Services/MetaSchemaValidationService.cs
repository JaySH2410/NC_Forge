using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;
using Forge.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Forge.Features.MetaSchema.Services;

//This Service is responsible for validating the new MetaSchema (Objects and Relationships)
//which is being created or updated or deleted; before it is persisted in the database.
// only creation for now
//It will check for the following:
//1. ValidateCreateObjectAsync
//2. ValidateCreateRelationshipAsync
public class MetaSchemaValidationService : IMetaSchemaValidationService
{
    private readonly AppDbContext _context;

    private readonly IMetaSchemaService _metaSchemaService;

    public MetaSchemaValidationService(
        AppDbContext context,
        IMetaSchemaService metaSchemaService)
    {
        _context = context;
        _metaSchemaService = metaSchemaService;
    }

    public async Task ValidateCreateObjectAsync(
        MetaObject newObject,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(newObject.Name)) { 
            throw new ValidationException(
                new Dictionary<string, string[]> { { "Name", ["Name is required."] } });
        }

        //Checking the DisplayName is not null or empty
        if (string.IsNullOrWhiteSpace(newObject.DisplayName))
        {
            newObject.DisplayName = newObject.Name;
        }

        //Checking for the duplicate Uuid 
        //We are generating the Uuid after this - so not required
        //if (await _metaSchemaService.ObjectExistsAsync(metaObject.Uuid, cancellationToken))
        //{
        //    throw new ValidationException(
        //        new Dictionary<string, string[]>
        //        {
        //            { "Uuid", [$"MetaObject with Uuid '{metaObject.Uuid}' already exists."] }
        //        });
        //}

        //Checking for the duplicate Name
        MetaObject? existingObject = await _context.MetaObjects
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name == newObject.Name,
                cancellationToken);

        if (existingObject is not null)
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "Name", [$"MetaObject with name '{newObject.Name}' already exists."] }
                });
        }

        //Checking for the Object Type Uid if it is provided, and if it exists in the database
        //If in case we are generating the Foundational Uuid, then we need to skip this
        //if (!metaObject.ObjTypeUid.HasValue)
        //{
        //    throw new ValidationException(
        //        new Dictionary<string, string[]> { { "ObjTypeUid", ["Object Type is required."] } });
        //}
        if (newObject.ObjTypeUid.HasValue)
        {
            var exists = await _metaSchemaService.ObjectExistsAsync(
                newObject.ObjTypeUid.Value,
                cancellationToken);

            if (!exists)
            {
                throw new ValidationException(
                    new Dictionary<string, string[]>
                    {
                    { "ObjTypeUid", [$"MetaObject with type '{newObject.ObjTypeUid}' does not exist."] }
                    });
            }
        }
        if (newObject.ObjTypeUid == newObject.Uuid)
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "ObjTypeUid", ["Object cannot reference itself as its type."] }
                });
        }
    }

    public async Task ValidateUpdateObjectAsync(
        MetaObject existingObject,
        UpdateMetaObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            request.DisplayName = existingObject.Name;
        }
        await Task.CompletedTask;
    }

    public async Task ValidateDeactivateObjectAsync(
        MetaObject existingObject,
        CancellationToken cancellationToken = default)
    {
        if (!existingObject.IsActive)
        {
            throw new BusinessException(
                $"Object '{existingObject.Uuid}' | '{existingObject.DisplayName}' is already deleted/inactive.");
        }
        await Task.CompletedTask;
    }

    public async Task ValidateActivateObjectAsync(
        MetaObject existingObject,
        CancellationToken cancellationToken = default)
    {
        if (existingObject.IsActive)
        {
            throw new BusinessException(
                $"Object '{existingObject.Uuid}' | '{existingObject.DisplayName}' is already restored/active.");
        }
        await Task.CompletedTask;
    }

    public async Task ValidateTerminateObjectAsync(
        MetaObject metaObject,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public async Task ValidateCreateRelationshipAsync(
        MetaObjectRelationship newRel,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(newRel.Name))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "Name", ["Name is required."] } });
        }

        //Checking the DisplayName is not null or empty
        if (string.IsNullOrWhiteSpace(newRel.DisplayName))
        {
            newRel.DisplayName = newRel.Name;
        }

        //Checking for the duplicate Uuid 
        //We are generating the Uuid after this - so not required
        //if (await _metaSchemaService.ObjectExistsAsync(relationship.Uuid, cancellationToken))
        //{
        //    throw new ValidationException(
        //        new Dictionary<string, string[]>
        //        {
        //            { "Uuid", [$"MetaRelationship with Uuid '{relationship.Uuid}' already exists."] }
        //        });
        //}

        // End1 must exist
        if (!await _metaSchemaService.ObjectExistsAsync(
                newRel.End1Uid,
                cancellationToken))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "End1Uid", [$"Source object '{newRel.End1Uid}' does not exist."] } });
        }

        // End2 must exist
        if (!await _metaSchemaService.ObjectExistsAsync(
                newRel.End2Uid,
                cancellationToken))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "End2Uid", [$"Target object '{newRel.End2Uid}' does not exist."] } });
        }

        //Relationship Type should not be null or empty
        if (string.IsNullOrWhiteSpace(newRel.RelTypeUid.ToString()))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "RelTypeUid", ["Relationship type is required."] } });
        }

        // Relationship Type must exist
        if (!await _metaSchemaService.ObjectExistsAsync(
                newRel.RelTypeUid,
                cancellationToken))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "RelTypeUid", [$"Relationship type '{newRel.RelTypeUid}' does not exist."] } });
        }

        // Duplicate relationship
        var relationshipExists = await _context.MetaObjectRelationships
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.End1Uid == newRel.End1Uid &&
                    x.End2Uid == newRel.End2Uid &&
                    x.RelTypeUid == newRel.RelTypeUid,
                cancellationToken);

        if (relationshipExists)
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "Relationship", ["An identical relationship already exists."] } });
        }
    }

    public async Task ValidateUpdateRelationshipAsync(
        MetaObjectRelationship existingRel,
        UpdateMetaObjectRelationshipRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            request.DisplayName = existingRel.Name;
        }
        await Task.CompletedTask;
    }

    public async Task ValidateDeactivateRelationshipAsync(
        MetaObjectRelationship existingRel,
        CancellationToken cancellationToken = default)
    {
        if (!existingRel.IsActive)
        {
            throw new BusinessException(
                $"Relationship '{existingRel.Uuid}' | '{existingRel.DisplayName}' is already deleted/inactive.");
        }
        await Task.CompletedTask;
    }

    public async Task ValidateActivateRelationshipAsync(
        MetaObjectRelationship existingRel,
        CancellationToken cancellationToken = default)
    {
        if (existingRel.IsActive)
        {
            throw new BusinessException(
                $"Relationship '{existingRel.Uuid}' | '{existingRel.DisplayName}' is already restored/active.");
        }
        await Task.CompletedTask;
    }

    public async Task ValidateTerminateRelationshipAsync(
        MetaObjectRelationship existingRel,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

}