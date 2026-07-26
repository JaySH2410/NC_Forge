using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Forge.Features.Auth.Entities;
using Forge.Features.MetaSchema.Entities;
using Forge.Shared.Entities;

namespace Forge.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    { }
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<EmailVerificationToken> EmailVerificationTokens =>Set<EmailVerificationToken>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<MetaObject> MetaObjects => Set<MetaObject>();
    public DbSet<MetaObjectRelationship> MetaObjectRelationships => Set<MetaObjectRelationship>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return await base.SaveChangesAsync(cancellationToken);
    }
    private void UpdateAuditFields()
    {
       var utcNow = DateTimeOffset.UtcNow;

       foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
       {
           switch (entry.State)
           {
               case EntityState.Added:
                   entry.Entity.CreatedAt = utcNow;
                   entry.Entity.CreatedBy = null; // Current user
                   break;

               case EntityState.Modified:
                   entry.Property(x => x.CreatedAt).IsModified = false;
                   entry.Property(x => x.CreatedBy).IsModified = false;

                   entry.Entity.UpdatedAt = utcNow;
                   entry.Entity.UpdatedBy = null; // Current user
                   break;

               case EntityState.Deleted:
                   entry.State = EntityState.Modified;

                   entry.Property(x => x.CreatedAt).IsModified = false;
                   entry.Property(x => x.CreatedBy).IsModified = false;

                   entry.Entity.DeletedAt = utcNow;
                   entry.Entity.DeletedBy = null; // Current user
                   break;
           }
       }
    }
}


