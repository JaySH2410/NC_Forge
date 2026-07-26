using Forge.Features.Auth.DTOs.Internal;
using Forge.Features.Auth.Entities;

namespace Forge.Features.Auth.Contracts;

public interface IJwtTokenService
{
    AccessTokenResult GenerateAccessToken(User user);
}
