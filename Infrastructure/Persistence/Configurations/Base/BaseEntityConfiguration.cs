using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Shared.Entities;

namespace Forge.Infrastructure.Persistence.Configurations.Base;

public abstract class BaseEntityConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Uuid)
            .ValueGeneratedNever()  
            .IsRequired();

        builder.HasIndex(x => x.Uuid)
            .IsUnique();
    }
}