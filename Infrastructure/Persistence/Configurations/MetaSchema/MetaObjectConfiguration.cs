using Forge.Features.MetaSchema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public class MetaObjectConfiguration: IEntityTypeConfiguration<MetaObject>
{
    public void Configure(EntityTypeBuilder<MetaObject> builder)
    {
        builder.ToTable("MetaObject");

        builder.Property(x => x.Uuid).HasColumnName("ObjUid");
    }
}