using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Forge.Features.Auth.Entities;

namespace Forge.Infrastructure.Persistence.Configurations.Auth;

public class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(
        EntityTypeBuilder<RefreshToken> builder)
    {
        
        builder.ToTable("RefreshToken");

    }
}