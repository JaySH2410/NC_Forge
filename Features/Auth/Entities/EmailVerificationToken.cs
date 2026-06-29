using test.Shared.Entities;

namespace test.Features.Auth.Entities;

public class EmailVerificationToken: AuditableEntity
{
    public required string TokenHash { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset? UsedAt { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
