using Forge.Features.MetaSchema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public class UuidCounterConfiguration : IEntityTypeConfiguration<UuidCounter>
{
    public void Configure(EntityTypeBuilder<UuidCounter> builder)
    {
        builder.ToTable("UuidCounter");
    }
}