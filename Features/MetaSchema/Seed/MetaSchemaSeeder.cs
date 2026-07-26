using Forge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Forge.Features.MetaSchema.Seed;

public static class MetaSchemaSeeder
{
    public static async Task SeedAsync(
        AppDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        await SeedApplicationsAsync(dbContext, cancellationToken);

        await SeedMetaObjectsAsync(dbContext, cancellationToken);

        await SeedMetaObjectRelationshipsAsync(dbContext, cancellationToken);
    }

    private static async Task SeedApplicationsAsync(
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (await dbContext.Applications.AnyAsync(cancellationToken))
            return;

        await dbContext.Applications.AddRangeAsync(
            ApplicationSeeder.GetApplications(),
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedMetaObjectsAsync(
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (await dbContext.MetaObjects.AnyAsync(cancellationToken))
            return;

        await dbContext.MetaObjects.AddRangeAsync(
            MetaObjectSeeder.GetMetaObjects(),
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedMetaObjectRelationshipsAsync(
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await dbContext.MetaObjectRelationships.AddRangeAsync(
            MetaObjectRelationshipSeeder.GetMetaObjectRelationships(),
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}