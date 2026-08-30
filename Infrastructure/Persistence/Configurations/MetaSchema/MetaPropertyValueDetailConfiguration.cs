using Forge.Features.MetaSchema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public sealed class MetaPropertyValueDetailConfiguration : IEntityTypeConfiguration<MetaPropertyValueDetail>
{
    public void Configure(EntityTypeBuilder<MetaPropertyValueDetail> builder)
    {
        builder.ToTable("MetaPropertyValueDetail");
        builder.HasKey(x => x.PropertyValueId);

        builder.HasOne(x => x.PropertyValue)
            .WithOne(x => x.Detail)
            .HasForeignKey<MetaPropertyValueDetail>(x => x.PropertyValueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}