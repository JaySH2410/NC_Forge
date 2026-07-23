using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test.Shared.Entities;

namespace test.Infrastructure.Persistence.Configurations.Base;

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