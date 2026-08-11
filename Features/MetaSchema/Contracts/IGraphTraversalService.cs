using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Contracts;
public interface IGraphTraversalService
{
    Task<IReadOnlyCollection<MetaObject>> GetTargetsAsync(
        Guid sourceUid,
        Guid relationshipTypeUid,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MetaObject>> GetSourcesAsync(
        Guid targetUid,
        Guid relationshipTypeUid,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MetaObjectRelationship>> GetOutgoingRelationshipsAsync(
        Guid sourceUid,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MetaObjectRelationship>> GetIncomingRelationshipsAsync(
        Guid targetUid,
        CancellationToken cancellationToken = default);
}