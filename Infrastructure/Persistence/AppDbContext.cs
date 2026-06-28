using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using test.Features.Auth.Entities;
using test.Shared.Contracts;
using test.Shared.Entities;

namespace test.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserService _currentUser;
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUserService currentUser
        )
        : base(options)
    { 
        _currentUser = currentUser;
    }
    public DbSet<User> Users => Set<User>();
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
        var entries = ChangeTracker.Entries<AuditableEntity>();

        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
                entry.Entity.CreatedBy = _currentUser.UserId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.CreatedAt).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;
                entry.Entity.UpdatedAt = utcNow;
                entry.Entity.UpdatedBy = _currentUser.UserId;
            }
        }
    }
}


