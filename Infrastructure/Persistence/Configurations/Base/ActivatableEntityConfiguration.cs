using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Shared.Entities;

namespace Forge.Infrastructure.Persistence.Configurations.Base;

public abstract class ActivatableEntityConfiguration<TEntity>
    : AuditableEntityConfiguration<TEntity>
    where TEntity : ActivatableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
    }
}

