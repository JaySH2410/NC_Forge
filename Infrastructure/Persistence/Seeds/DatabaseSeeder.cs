using Forge.Features.MetaSchema.Seed;
using Forge.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace  Forge.Infrastructure.Persistence.Seeds;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var settings = scope.ServiceProvider
            .GetRequiredService<IOptions<DatabaseSettings>>()
            .Value;

        //if (settings.ReseedMetaSchema)
        //{
        //    await dbContext.MetaObjectRelationships.ExecuteDeleteAsync();
        //    await dbContext.MetaObjects.ExecuteDeleteAsync();
        //    await dbContext.Applications.ExecuteDeleteAsync();
        //}
        await dbContext.Database.MigrateAsync();

        await MetaSchemaSeeder.SeedAsync(dbContext);

        // Future
        // await AuthSeeder.SeedAsync(dbContext);
        // await AdminConfigSeeder.SeedAsync(dbContext);
    }
}