using Forge.Features.Auth.DTOs.Internal;
using Forge.Features.Auth.Entities;

namespace Forge.Features.Auth.Contracts;

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
