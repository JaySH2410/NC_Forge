using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test.Features.Auth.Constants;
using test.Features.Auth.Entities;
using test.Infrastructure.Persistence.Configurations.Base;

namespace test.Infrastructure.Persistence.Configurations;

public class UserConfiguration : ActivatableEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("User");

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(UserConstants.FirstNameMaxLength);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(UserConstants.LastNameMaxLength);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(UserConstants.EmailMaxLength);

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.IsEmailVerified)
            .HasDefaultValue(false);

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}