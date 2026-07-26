using Forge.Features.MetaSchema.Constants;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Seed;

public class ApplicationSeeder
{
    public static IEnumerable<Application> GetApplications()
    {
        return
        [
            new Application
            {
                Uuid = MetaSchemaConstants.Applications.CoreForge,
                Name = "CoreForge",
                DisplayName = "Core Forge",
                Description = "Core Forge Platform",
                Version = MetaSchemaConstants.Applications.CurrentVersion,
                IsActive = true
            }
        ];
    }
}