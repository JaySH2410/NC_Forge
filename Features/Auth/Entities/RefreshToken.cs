using test.Shared.Entities;

namespace test.Features.Auth.Entities;

public class RefreshToken : AuditableEntity
{
    public required string TokenHash { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    public string? ReplacedByTokenHash { get; set; }

    public string? RevokedReason { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}