using Forge.Features.MetaSchema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public sealed class MetaObjectRelationshipConfiguration : IEntityTypeConfiguration<MetaObjectRelationship>
{
    public void Configure(EntityTypeBuilder<MetaObjectRelationship> builder)
    {
       builder.ToTable("MetaObjectRelationship");
        builder.HasKey(x => x.Id);

       builder.Property(x => x.Uuid).HasColumnName("RelUid");

        builder.HasIndex(x => x.Uuid).IsUnique();
        builder.HasIndex(x => x.RelTypeUid);
        builder.HasIndex(x => x.End1Uid);
        builder.HasIndex(x => x.End2Uid);
        builder.HasIndex(x => new { x.RelTypeUid, x.End1Uid });
        builder.HasIndex(x => new { x.RelTypeUid, x.End2Uid });
        builder.HasIndex(x => new { x.RelTypeUid, x.End1Uid, x.End2Uid });

        // RelTypeUid -> MetaObject.ObjUid
        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.RelTypeUid)
            .HasPrincipalKey(x => x.Uuid)
            .IsRequired()   // flip to false if you keep RelTypeUid nullable in SSDT
            .OnDelete(DeleteBehavior.Restrict);

        // End1Uid -> MetaObject.ObjUid
        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.End1Uid)
            .HasPrincipalKey(x => x.Uuid)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // End2Uid -> MetaObject.ObjUid
        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.End2Uid)
            .HasPrincipalKey(x => x.Uuid)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}