using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Features.Auth.Constants;
using Forge.Features.Auth.Entities;
using Forge.Infrastructure.Persistence.Configurations.Base;

namespace Forge.Infrastructure.Persistence.Configurations.Auth;

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