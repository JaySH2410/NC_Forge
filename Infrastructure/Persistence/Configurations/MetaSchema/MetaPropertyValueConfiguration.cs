using Forge.Features.MetaSchema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public sealed class MetaPropertyValueConfiguration : IEntityTypeConfiguration<MetaPropertyValue>
{
    public void Configure(EntityTypeBuilder<MetaPropertyValue> builder)
    {
        builder.ToTable("MetaPropertyValue");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Uuid).HasColumnName("PrUid");

        builder.HasIndex(x => x.Uuid).IsUnique();
        builder.HasIndex(x => new { x.IfUid, x.PropertyUid });
        builder.HasIndex(x => x.PropertyUid);

        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.ObjUid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MetaInterface>()
            .WithMany()
            .HasForeignKey(x => new { x.ObjUid, x.IfUid })
            .HasPrincipalKey(x => new { x.ObjUid, x.Uuid })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.PropertyUid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}