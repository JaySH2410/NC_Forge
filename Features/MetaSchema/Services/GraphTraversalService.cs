using Microsoft.EntityFrameworkCore;
using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;

namespace Forge.Features.MetaSchema.Services;

//This service is responsible for traversing the MetaSchema graph (Objects and Relationships).
//It will provide the following functionalities:
//1. Get all source MetaObjects by target Uid and relationship type Uid
//2. Get all target MetaObjects by source Uid and relationship type Uid
//3. Get all incoming MetaObjectRelationships by target Uid(END2)
//4. Get all outgoing MetaObjectRelationships by source Uid(END1)

public class GraphTraversalService : IGraphTraversalService
{
    private readonly AppDbContext _context;

    public GraphTraversalService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<MetaObject>>
        GetSourcesAsync(
            Guid targetUid,
            Guid relationshipTypeUid,
            CancellationToken cancellationToken = default)
    {
        return await
        (
            from relationship in _context.MetaObjectRelationships.AsNoTracking()
            join source in _context.MetaObjects.AsNoTracking()
                on relationship.End1Uid equals source.Uuid
            where relationship.End2Uid == targetUid
                  && relationship.RelTypeUid == relationshipTypeUid
            select source
        ).ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyCollection<MetaObject>>
        GetTargetsAsync(
            Guid sourceUid,
            Guid relationshipTypeUid,
            CancellationToken cancellationToken = default)
    {
        return await
        (
            from relationship in _context.MetaObjectRelationships.AsNoTracking()
            join target in _context.MetaObjects.AsNoTracking()
                on relationship.End2Uid equals target.Uuid
            where relationship.End1Uid == sourceUid
                  && relationship.RelTypeUid == relationshipTypeUid
            select target
        ).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<MetaObjectRelationship>>
        GetIncomingRelationshipsAsync(
            Guid targetUid,
            CancellationToken cancellationToken = default)
    {
        return await _context.MetaObjectRelationships
            .AsNoTracking()
            .Where(x => x.End2Uid == targetUid)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<MetaObjectRelationship>>
        GetOutgoingRelationshipsAsync(
            Guid sourceUid,
            CancellationToken cancellationToken = default)
    {
        return await _context.MetaObjectRelationships
            .AsNoTracking()
            .Where(x => x.End1Uid == sourceUid)
            .ToListAsync(cancellationToken);
    }
}