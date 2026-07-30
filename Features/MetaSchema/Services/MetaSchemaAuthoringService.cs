using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;

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
        MetaObject metaObject,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeactivateObjectAsync(
        Guid objUid,
        CancellationToken cancellationToken = default){
        throw new NotImplementedException();
    }
    
    public async Task TerminateObjectAsync(
        Guid objUid,
        CancellationToken cancellationToken = default){
        throw new NotImplementedException();
    }

    public async Task<MetaObjectRelationship> CreateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateCreateRelationshipAsync(
            relationship,
            cancellationToken);

        _context.MetaObjectRelationships.Add(relationship);

        await _context.SaveChangesAsync(cancellationToken);

        return relationship;
    }

    public async Task<MetaObjectRelationship> UpdateRelationshipAsync(
        MetaObjectRelationship relationship,
        CancellationToken cancellationToken = default){
        throw new NotImplementedException();
    }
    
    public async Task DeactivateRelationshipAsync(
        Guid relationshipUid,
        CancellationToken cancellationToken = default){
        throw new NotImplementedException();
    }
    
    public async Task TerminateRelationshipAsync(
        Guid relationshipUid,
        CancellationToken cancellationToken = default){
        throw new NotImplementedException();
    }
}