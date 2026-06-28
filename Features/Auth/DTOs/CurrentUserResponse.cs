namespace test.Features.Auth.DTOs;

public class CurrentUserResponse
{
    public int Id { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Email { get; init; }

    public bool IsEmailVerified { get; init; }

    public DateTimeOffset? LastLoginAt { get; init; }
}