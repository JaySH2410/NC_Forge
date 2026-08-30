namespace Forge.Features.Auth.DTOs.Response;

public class CurrentUserResponse
{
    public long Id { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Email { get; init; }

    public bool IsEmailVerified { get; init; }

    public DateTimeOffset? LastLoginAt { get; init; }
}