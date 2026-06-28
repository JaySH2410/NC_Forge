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
    private readonly ICurrentUserService _currentUser;

    public AuthService(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ICurrentUserService currentUser
        )
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
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
            throw new BusinessException(
                AuthErrorMessages.InvalidCredentials);
        }

        var isValidPassword =_passwordHasher.VerifyPassword(request.Password,user.PasswordHash);

        if (!isValidPassword)
        {
            throw new BusinessException(
                AuthErrorMessages.InvalidCredentials);
        }

        var tokenResult = _jwtTokenService.GenerateAccessToken(user);

        return new LoginResponse
        {
            AccessToken = tokenResult.AccessToken,
            ExpiresAt = tokenResult.ExpiresAt
        };

    }

    public async Task<CurrentUserResponse> GetCurrentUserAsync(
    CancellationToken cancellationToken = default)
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
    private static string NormalizeEmail(string email)
    {
        return email
            .Trim()
            .ToLowerInvariant();
    }
}