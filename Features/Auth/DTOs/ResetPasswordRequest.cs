namespace test.Features.Auth.DTOs;

public class ResetPasswordRequest
{
    public required string Token { get; init; }

    public required string NewPassword { get; init; }

    public required string ConfirmPassword { get; init; }
}
