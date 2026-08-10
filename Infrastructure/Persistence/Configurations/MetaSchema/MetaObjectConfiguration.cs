using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public class MetaObjectConfiguration: IEntityTypeConfiguration<MetaObject>
{
    public void Configure(EntityTypeBuilder<MetaObject> builder)
    {
        builder.ToTable("MetaObject"); 
    }
}