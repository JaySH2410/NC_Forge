using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.Constants;
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
            var typeObject = await _context.MetaObjects
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Uuid == newObject.ObjTypeUid.Value,
                    cancellationToken);

            if (typeObject is null)
            {
                throw new ValidationException(
                    new Dictionary<string, string[]>
                    {
                        { "ObjTypeUid", [$"Object type '{newObject.ObjTypeUid}' does not exist."] }
                    });
            }

            if (!typeObject.IsActive)
            {
                throw new ValidationException(
                    new Dictionary<string, string[]>
                    {
                        { "ObjTypeUid", [$"Object type '{newObject.ObjTypeUid}' is inactive."] }
                    });
            }

            if (typeObject.ObjTypeUid.HasValue)
            {
                throw new ValidationException(
                    new Dictionary<string, string[]>
                    {
                        { "ObjTypeUid", ["ObjTypeUid must reference a root object whose ObjTypeUid is null."] }
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

        if (newRel.RelTypeUid == newRel.End1Uid ||
            newRel.RelTypeUid == newRel.End2Uid)
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "RelTypeUid", ["Relationship type cannot be the same as either relationship endpoint."] }
                });
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

    public void ValidateGenericRelationshipAuthoringAllowed(
        Guid relationshipTypeUid)
    {
        if (relationshipTypeUid != MetaSchemaConstants.RelationshipTypes.Implements &&
            relationshipTypeUid != MetaSchemaConstants.RelationshipTypes.PrimaryInterface)
        {
            return;
        }

        throw new ValidationException(
            new Dictionary<string, string[]>
            {
                {
                    "RelTypeUid",
                    [
                        "Implements and PrimaryInterface relationships must be managed " +
                        "through /api/MetaSchema/interface-implementations."
                    ]
                }
            });
    }

    public async Task ValidateCreateInterfaceImplementationAsync(
        CreateInterfaceImplementationRequest request,
        CancellationToken cancellationToken = default)
    {
        var objects = await _context.MetaObjects
            .AsNoTracking()
            .Where(x => x.Uuid == request.ObjUid || x.Uuid == request.InterfaceUid)
            .ToListAsync(cancellationToken);

        var implementingObject = objects.FirstOrDefault(x => x.Uuid == request.ObjUid);
        if (implementingObject is null)
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "ObjUid", [$"Object '{request.ObjUid}' does not exist."] } });
        }

        var interfaceObject = objects.FirstOrDefault(x => x.Uuid == request.InterfaceUid);
        if (interfaceObject is null)
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "InterfaceUid", [$"Interface '{request.InterfaceUid}' does not exist."] } });
        }

        if (!implementingObject.IsActive)
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "ObjUid", [$"Object '{request.ObjUid}' is inactive."] } });
        }

        if (!interfaceObject.IsActive)
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "InterfaceUid", [$"Interface '{request.InterfaceUid}' is inactive."] } });
        }

        if (implementingObject.ObjTypeUid != MetaSchemaConstants.ObjectTypes.Class

            // !await IsObjectOfTypeAsync(
            //     implementingObject,
            //     MetaSchemaConstants.ObjectTypes.Class,
            //     cancellationToken)
            )
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "ObjUid", ["The implementing object must be Class or derive from Class."] }
                });
        }

        if (interfaceObject.ObjTypeUid != MetaSchemaConstants.ObjectTypes.Interface
            // !await IsObjectOfTypeAsync(
            //     interfaceObject,
            //     MetaSchemaConstants.ObjectTypes.Interface,
            //     cancellationToken)
            )
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "InterfaceUid", ["The interface must be Interface or derive from Interface."] }
                });
        }

        if (await _context.MetaInterfaces.AsNoTracking().AnyAsync(
                x => x.ObjUid == request.ObjUid && x.InterfaceUid == request.InterfaceUid,
                cancellationToken))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "InterfaceImplementation", ["This object already implements the interface."] } });
        }

        if (request.IsPrimary && await _context.MetaInterfaces.AsNoTracking().AnyAsync(
                x => x.ObjUid == request.ObjUid && x.IsPrimary && x.IsActive,
                cancellationToken))
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "IsPrimary", ["The object already has a primary interface."] } });
        }
    }

    // private async Task<bool> IsObjectOfTypeAsync(
    //     MetaObject metaObject,
    //     Guid requiredTypeUid,
    //     CancellationToken cancellationToken)
    // {
    //     var currentUid = metaObject.Uuid;
    //     var currentTypeUid = metaObject.ObjTypeUid;
    //     var visited = new HashSet<Guid>();

    //     while (visited.Add(currentUid))
    //     {
    //         if (currentUid == requiredTypeUid)
    //         {
    //             return true;
    //         }

    //         if (!currentTypeUid.HasValue)
    //         {
    //             return false;
    //         }

    //         var parentType = await _context.MetaObjects
    //             .AsNoTracking()
    //             .Where(x => x.Uuid == currentTypeUid.Value)
    //             .Select(x => new { x.Uuid, x.ObjTypeUid })
    //             .SingleOrDefaultAsync(cancellationToken);

    //         if (parentType is null)
    //         {
    //             return false;
    //         }

    //         currentUid = parentType.Uuid;
    //         currentTypeUid = parentType.ObjTypeUid;
    //     }

    //     return false;
    // }

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
