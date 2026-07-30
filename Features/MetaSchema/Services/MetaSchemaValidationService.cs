using Forge.Features.MetaSchema.Contracts;
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
        MetaObject metaObject,
        CancellationToken cancellationToken = default)
    {
        //Checking the DisplayName is not null or empty
        if (string.IsNullOrWhiteSpace(metaObject.DisplayName))
        {
            metaObject.DisplayName = metaObject.Name;
        }

        //Checking for the duplicate Uuid 
        if (await _metaSchemaService.ExistsAsync(metaObject.Uuid, cancellationToken))
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "Uuid", [$"MetaObject with Uuid '{metaObject.Uuid}' already exists."] }
                });
        }

        //Checking for the duplicate Name
        MetaObject? existingObject = await _context.MetaObjects
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name == metaObject.Name,
                cancellationToken);

        if (existingObject is not null)
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "Name", [$"MetaObject with name '{metaObject.Name}' already exists."] }
                });
        }

        //Checking for the Object Type Uid if it is provided, and if it exists in the database
        if (!metaObject.ObjTypeUid.HasValue)
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "ObjTypeUid", ["Object Type is required."] } });
        }

        var exists = await _metaSchemaService.ExistsAsync(
            metaObject.ObjTypeUid.Value,
            cancellationToken);

        if (!exists)
        {
            throw new NotFoundException($"MetaObject with type '{metaObject.ObjTypeUid}' does not exist.");
        }

        if (metaObject.ObjTypeUid == metaObject.Uuid)
        {
            throw new ValidationException(
                new Dictionary<string, string[]>
                {
                    { "ObjTypeUid", ["Object cannot reference itself as its type."] }
                });
        }
    }

    public async Task ValidateCreateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default)
    {
        // End1 must exist
        if (!await _metaSchemaService.ExistsAsync(
                relationship.End1Uid,
                cancellationToken))
        {
            throw new NotFoundException(
                $"Source object '{relationship.End1Uid}' does not exist.");
        }

        // End2 must exist
        if (!await _metaSchemaService.ExistsAsync(
                relationship.End2Uid,
                cancellationToken))
        {
            throw new NotFoundException(
                $"Target object '{relationship.End2Uid}' does not exist.");
        }

        // Relationship Type must exist
        if (!await _metaSchemaService.ExistsAsync(
                relationship.RelTypeUid,
                cancellationToken))
        {
            throw new NotFoundException(
                $"Relationship type '{relationship.RelTypeUid}' does not exist.");
        }

        // Duplicate relationship
        var relationshipExists = await _context.MetaObjectRelationships
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.End1Uid == relationship.End1Uid &&
                    x.End2Uid == relationship.End2Uid &&
                    x.RelTypeUid == relationship.RelTypeUid,
                cancellationToken);

        if (relationshipExists)
        {
            throw new ValidationException(
                new Dictionary<string, string[]> { { "Relationship", ["An identical relationship already exists."] } });
        }
    }
}