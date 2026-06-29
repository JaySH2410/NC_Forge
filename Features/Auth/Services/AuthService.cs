using Microsoft.EntityFrameworkCore;
using test.Features.Auth.Constants;
using test.Features.Auth.Contracts;
using test.Features.Auth.DTOs;
using test.Features.Auth.Entities;
using test.Features.Auth.Services;
using test.Infrastructure.Persistence;
using test.Shared.Contracts;
using test.Shared.Exceptions;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ICurrentUserService _currentUser;

    public AuthService(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService,
        ICurrentUserService currentUser
        )
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
        _currentUser = currentUser;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request,CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(request.Email);

        var emailExists = await _dbContext.Users
        .AsNoTracking()
        .AnyAsync(
            x => x.Email == normalizedEmail,
            cancellationToken);

        if (emailExists)
        {
            throw new BusinessException(AuthErrorMessages.EmailAlreadyExists);
        }

        var passwordHash =_passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            IsEmailVerified = false
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        throw new NotImplementedException();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request,CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(request.Email);

        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(
            x => x.Email == normalizedEmail,
            cancellationToken
        );

        if (user is null)
        {
            throw new BusinessException(AuthErrorMessages.InvalidCredentials);
        }

        var isValidPassword =_passwordHasher.VerifyPassword(request.Password,user.PasswordHash);

        if (!isValidPassword)
        {
            throw new BusinessException(AuthErrorMessages.InvalidCredentials);
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(user);

        var refreshToken = _refreshTokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            TokenHash = refreshToken.TokenHash,
            ExpiresAt = refreshToken.ExpiresAt,
            UserId = user.Id
        };

        await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity,cancellationToken);

        user.LastLoginAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResponse { 
            Tokens = new TokenResult
            {
                AccessToken = accessToken.AccessToken,
                AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshToken = refreshToken.RefreshToken,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt
            }
        };
        
    }

    public async Task<CurrentUserResponse> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUser.UserId;
        if (currentUserId is null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.Unauthorized);
        }

        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == currentUserId,
                cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                AuthErrorMessages.UserNotFound);
        }

        return new CurrentUserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsEmailVerified = user.IsEmailVerified,
            LastLoginAt = user.LastLoginAt
        };
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(
    RefreshTokenRequest request,
    CancellationToken cancellationToken = default)
    {
        var tokenHash = _refreshTokenService.HashToken(
            request.RefreshToken);

        var refreshToken = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (refreshToken is null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.InvalidRefreshToken);
        }

        if (refreshToken.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.RefreshTokenExpired);
        }

        if (refreshToken.RevokedAt is not null)
        {
            throw new UnauthorizedException(
                AuthErrorMessages.RefreshTokenRevoked);
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(refreshToken.User);

        var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            TokenHash = newRefreshToken.TokenHash,
            ExpiresAt = newRefreshToken.ExpiresAt,
            UserId = refreshToken.UserId
        };

        refreshToken.RevokedAt = DateTimeOffset.UtcNow;
        refreshToken.ReplacedByTokenHash = newRefreshToken.TokenHash;
        refreshToken.RevokedReason = RefreshTokenMessages.TokenRotated;

        await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity,cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return new RefreshTokenResponse
        {
            Tokens = new TokenResult
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = newRefreshToken.RefreshToken,
                AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            }
        };

    }
    private static string NormalizeEmail(string email)
    {
        return email
            .Trim()
            .ToLowerInvariant();
    }
}