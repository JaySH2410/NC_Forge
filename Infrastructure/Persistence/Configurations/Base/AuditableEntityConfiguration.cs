using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Shared.Entities;

namespace Forge.Infrastructure.Persistence.Configurations.Base;

public abstract class AuditableEntityConfiguration<TEntity>
    : BaseEntityConfiguration<TEntity>
    where TEntity : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy);

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.UpdatedBy);

        builder.Property(x => x.DeletedAt);
        
        builder.Property(x => x.DeletedBy);
    }
}