using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Shared.Entities;

namespace Forge.Infrastructure.Persistence.Configurations.Base;

public abstract class NamedEntityConfiguration<TEntity>
    : ActivatableEntityConfiguration<TEntity>   
    where TEntity : NamedEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(x => x.DisplayName)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(4000); // or .HasColumnType("nvarchar(max)")

        // builder.HasIndex(x => x.Name)
        //     .IsUnique();
    }
}
