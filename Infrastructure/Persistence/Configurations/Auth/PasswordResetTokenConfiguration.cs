using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Features.Auth.Entities;

namespace Forge.Infrastructure.Persistence.Configurations.Auth;

public class PasswordResetTokenConfiguration
    : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(
        EntityTypeBuilder<PasswordResetToken> builder)
    {
        
        builder.ToTable("PasswordResetToken");
    }
}