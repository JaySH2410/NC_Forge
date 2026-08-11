using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Features.Auth.Entities;

namespace Forge.Infrastructure.Persistence.Configurations.Auth;

public class EmailVerificationTokenConfiguration
    : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(
        EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.ToTable("EmailVerificationToken");
    }
}