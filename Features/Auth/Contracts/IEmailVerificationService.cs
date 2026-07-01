using test.Features.Auth.DTOs.Internal;
using test.Features.Auth.Entities;

namespace test.Features.Auth.Contracts;

public interface IEmailVerificationService
{
    EmailVerificationTokenResult GenerateToken();

    string HashToken(
        string token);

    bool VerifyToken(
        string token,
        string tokenHash);

    Task<EmailVerificationToken> GetValidTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    void MarkAsUsed(
        EmailVerificationToken token);
}