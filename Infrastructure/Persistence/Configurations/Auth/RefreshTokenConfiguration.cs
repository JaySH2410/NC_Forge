using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Features.Auth.Entities;
using Forge.Infrastructure.Persistence.Configurations.Base;

namespace Forge.Infrastructure.Persistence.Configurations.Auth;

public class RefreshTokenConfiguration
    : AuditableEntityConfiguration<RefreshToken>
{
    public override void Configure(
        EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("RefreshToken");

        builder.Property(x => x.TokenHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.ReplacedByTokenHash)
            .HasMaxLength(64);

        builder.Property(x => x.RevokedReason)
            .HasMaxLength(500);

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TokenHash)
            .IsUnique();

        builder.HasIndex(x => x.UserId);
    }
}