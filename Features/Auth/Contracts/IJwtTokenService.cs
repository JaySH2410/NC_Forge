using test.Features.Auth.DTOs;
using test.Features.Auth.Entities;

namespace test.Features.Auth.Contracts;

public interface IJwtTokenService
{
    AccessTokenResult GenerateAccessToken(User user);
}
