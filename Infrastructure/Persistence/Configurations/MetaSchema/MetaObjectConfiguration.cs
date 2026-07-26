using Forge.Features.MetaSchema.Entities;
using Forge.Infrastructure.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public class MetaObjectConfiguration: NamedEntityConfiguration<MetaObject>
{
    public override void Configure(EntityTypeBuilder<MetaObject> builder)
    {
        base.Configure(builder);

        builder.ToTable("MetaObject"); 
        
        // Map BaseEntity.Uuid -> ObjUid
        builder.Property(x => x.Uuid)
            .HasColumnName("ObjUid");
        
        builder.HasAlternateKey(x => x.Uuid);

        // Object Type
        builder.Property(x => x.ObjTypeUid);
            // .IsRequired();

        builder.Property(x => x.ApplicationUid)
            .IsRequired();

        builder.HasIndex(x => x.ApplicationUid);

        builder.HasOne<Application>()
            .WithMany()
            .HasForeignKey(x => x.ApplicationUid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Version)
            .HasMaxLength(40)
            .IsRequired();

        builder.HasIndex(x => x.ObjTypeUid);

        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.ObjTypeUid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}