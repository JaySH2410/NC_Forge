using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test.Features.Auth.Entities;
using test.Infrastructure.Persistence.Configurations.Base;

namespace test.Infrastructure.Persistence.Configurations;

public class PasswordResetTokenConfiguration
    : AuditableEntityConfiguration<PasswordResetToken>
{
    public override void Configure(
        EntityTypeBuilder<PasswordResetToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("PasswordResetToken");

        builder.Property(x => x.TokenHash)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.PasswordResetTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TokenHash)
            .IsUnique();

        builder.HasIndex(x => x.UserId);
    }
}