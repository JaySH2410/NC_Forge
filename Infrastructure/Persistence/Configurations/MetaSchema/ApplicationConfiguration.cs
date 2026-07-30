using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public class ApplicationConfiguration: NamedEntityConfiguration<Application>
{
    public override void Configure(EntityTypeBuilder<Application> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Version)
            .HasMaxLength(40)
            .IsRequired();
    }
}