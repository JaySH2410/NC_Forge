using Forge.Features.MetaSchema.Seed;
using Forge.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Forge.Features.MetaSchema.Entities;

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

        //var entityType = dbContext.Model.FindEntityType(typeof(Application));

        //Console.WriteLine(entityType?.GetTableName());

        //Console.WriteLine(entityType?.GetTableName());

        if (settings.ReseedMetaSchema)
        {
            //await dbContext.MetaObjectRelationships.ExecuteDeleteAsync();
            //await dbContext.MetaObjects.ExecuteDeleteAsync();
            //await dbContext.Applications.ExecuteDeleteAsync();
        }

        await MetaSchemaSeeder.SeedAsync(dbContext);

        // Future
        // await AuthSeeder.SeedAsync(dbContext);
        // await AdminConfigSeeder.SeedAsync(dbContext);
    }
}