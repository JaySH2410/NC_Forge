using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public sealed class MetaObjectRelationshipConfiguration : NamedEntityConfiguration<MetaObjectRelationship>
{
    public override void Configure(EntityTypeBuilder<MetaObjectRelationship> builder)
    {
        base.Configure(builder);

        builder.ToTable("MetaObjectRelationship");

        // Map BaseEntity.Uuid -> RelUid
        builder.Property(x => x.Uuid)
            .HasColumnName("RelUid");

        builder.HasAlternateKey(x => x.Uuid);

        builder.Property(x => x.End1Uid)
            .IsRequired();

        builder.Property(x => x.End2Uid)
            .IsRequired();

        builder.Property(x => x.RelTypeUid)
            .IsRequired();

        builder.Property(x => x.Ordinal)
            .HasDefaultValue(0);

        // End1
        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.End1Uid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);

        // End2
        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.End2Uid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship Type
        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.RelTypeUid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.End1Uid);

        builder.HasIndex(x => x.End2Uid);

        builder.HasIndex(x => x.RelTypeUid);

        builder.HasIndex(x => new { x.End1Uid, x.RelTypeUid });

        builder.HasIndex(x => new { x.End2Uid, x.RelTypeUid });
        
        builder.HasIndex(x => new { x.End1Uid, x.RelTypeUid, x.End2Uid })
            .IsUnique();
    }
}