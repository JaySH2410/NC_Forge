namespace test.Features.Auth.DTOs;

public class ChangePasswordRequest
{
    public required string CurrentPassword { get; init; }

    public required string NewPassword { get; init; }

    public required string ConfirmPassword { get; init; }
}