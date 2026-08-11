using Forge.Features.MetaSchema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public sealed class MetaObjectRelationshipConfiguration : IEntityTypeConfiguration<MetaObjectRelationship>
{
    public void Configure(EntityTypeBuilder<MetaObjectRelationship> builder)
    {
       builder.ToTable("MetaObjectRelationship");
       builder.Property(x => x.Uuid).HasColumnName("RelUid");


    }
}