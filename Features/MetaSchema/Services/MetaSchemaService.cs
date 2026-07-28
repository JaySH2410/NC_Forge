using Microsoft.EntityFrameworkCore;
using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence;

namespace Forge.Features.MetaSchema.Services;

//This service is responsible for exposing the MetaSchema (Objects and Relationships).
//It will provide the following functionalities:
//1. Get MetaObject by Uid
//2. Get MetaObject by Name
//3. Check if MetaObject exists by Uid
//4. Get related(END2) MetaObjects by source Uid and relationship type Uid
//5. Get referencing(END1) MetaObjects by target Uid and relationship type Uid
//6. Get single related(END2) MetaObject by source Uid and relationship type Uid
public class MetaSchemaService : IMetaSchemaService
{
   private readonly IGraphTraversalService _graphTraversalService;

   private readonly AppDbContext _context;

   public MetaSchemaService(
       AppDbContext context,
       IGraphTraversalService graphTraversalService)
   {
       _context = context;
       _graphTraversalService = graphTraversalService;
   }

   public async Task<MetaObject?> GetObjectAsync(
       Guid objUid,
       CancellationToken cancellationToken = default)
   {
       return await _context.MetaObjects.FindAsync(
           [objUid],
           cancellationToken);
   }

   public async Task<MetaObject?> GetObjectByNameAsync(
       string name,
       CancellationToken cancellationToken = default)
   {
       return await _context.MetaObjects
           .AsNoTracking()
           .FirstOrDefaultAsync(
               x => x.Name == name,
               cancellationToken);
   }

   public async Task<bool> ExistsAsync(
       Guid objUid,
       CancellationToken cancellationToken = default)
   {
       return await _context.MetaObjects
           .AsNoTracking()
           .AnyAsync(
               x => x.Uuid == objUid,
               cancellationToken);
   }

   public async Task<IReadOnlyCollection<MetaObject>> GetRelatedObjectsAsync(
       Guid sourceUid,
       Guid relationshipTypeUid,
       CancellationToken cancellationToken = default)
   {
       return await _graphTraversalService.GetTargetsAsync(
           sourceUid,
           relationshipTypeUid,
           cancellationToken);
   }

   public async Task<IReadOnlyCollection<MetaObject>> GetReferencingObjectsAsync(
       Guid targetUid,
       Guid relationshipTypeUid,
       CancellationToken cancellationToken = default)
   {
       return await _graphTraversalService.GetSourcesAsync(
           targetUid,
           relationshipTypeUid,
           cancellationToken);
   }

   public async Task<MetaObject?> GetSingleRelatedObjectAsync(
       Guid sourceUid,
       Guid relationshipTypeUid,
       CancellationToken cancellationToken = default)
   {
       var result = await _graphTraversalService.GetTargetsAsync(
           sourceUid,
           relationshipTypeUid,
           cancellationToken);

       return result.FirstOrDefault();
   }
   
}