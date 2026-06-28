using test.Features.Auth.DTOs;

namespace test.Features.Auth.Contracts;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);

    Task<CurrentUserResponse> GetCurrentUserAsync(
        CancellationToken cancellationToken = default);
}
