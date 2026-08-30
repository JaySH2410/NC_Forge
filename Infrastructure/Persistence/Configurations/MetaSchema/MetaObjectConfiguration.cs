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

        builder.HasIndex(x => x.Uuid).IsUnique();
        builder.HasIndex(x => x.ObjTypeUid);
        builder.HasIndex(x => x.ApplicationUid);
        builder.HasIndex(x => x.IsActive);

        //ObjTypeUid->MetaObject.ObjUid(nullable — root types have no type)
        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.ObjTypeUid)
            .HasPrincipalKey(x => x.Uuid)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUid -> Application.Uuid
        builder.HasOne<Application>()
            .WithMany()
            .HasForeignKey(x => x.ApplicationUid)
            .HasPrincipalKey(x => x.Uuid)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}