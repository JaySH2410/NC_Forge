using Forge.Features.MetaSchema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forge.Infrastructure.Persistence.Configurations.MetaSchema;

public sealed class MetaInterfaceConfiguration : IEntityTypeConfiguration<MetaInterface>
{
    public void Configure(EntityTypeBuilder<MetaInterface> builder)
    {
        builder.ToTable("MetaInterface");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Uuid).HasColumnName("IfUid");

        builder.HasIndex(x => x.Uuid).IsUnique();
        builder.HasIndex(x => new { x.ObjUid, x.Uuid }).IsUnique();
        builder.HasIndex(x => new { x.ObjUid, x.InterfaceUid }).IsUnique();
        builder.HasIndex(x => x.InterfaceUid);

        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.ObjUid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MetaObject>()
            .WithMany()
            .HasForeignKey(x => x.InterfaceUid)
            .HasPrincipalKey(x => x.Uuid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}