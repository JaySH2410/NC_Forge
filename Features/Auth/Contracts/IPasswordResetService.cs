using test.Features.Auth.DTOs;
using test.Features.Auth.Entities;

namespace test.Features.Auth.Contracts;

public interface IPasswordResetService
{
    PasswordResetTokenResult GenerateToken();

    string HashToken(string token);

    bool VerifyToken(
        string token,
        string tokenHash);

    Task<PasswordResetToken> GetValidTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    void MarkAsUsed(
        PasswordResetToken token);
}
