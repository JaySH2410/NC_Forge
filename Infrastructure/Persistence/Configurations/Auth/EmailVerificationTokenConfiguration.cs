using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Features.Auth.Entities;
using Forge.Infrastructure.Persistence.Configurations.Base;

namespace Forge.Infrastructure.Persistence.Configurations.Auth;

public class EmailVerificationTokenConfiguration
    : AuditableEntityConfiguration<EmailVerificationToken>
{
    public override void Configure(
        EntityTypeBuilder<EmailVerificationToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("EmailVerificationToken");

        builder.Property(x => x.TokenHash)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.EmailVerificationTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TokenHash)
            .IsUnique();

        builder.HasIndex(x => x.UserId);

    }
}