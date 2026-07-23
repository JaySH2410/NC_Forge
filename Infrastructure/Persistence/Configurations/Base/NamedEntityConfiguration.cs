using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test.Shared.Entities;

namespace test.Infrastructure.Persistence.Configurations.Base;

public abstract class NamedEntityConfiguration<TEntity>
    : ActivatableEntityConfiguration<TEntity>   
    where TEntity : NamedEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired();

        // builder.HasIndex(x => x.Name)
        //     .IsUnique();
    }
}
