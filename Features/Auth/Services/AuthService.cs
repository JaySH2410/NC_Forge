using Microsoft.EntityFrameworkCore;
using test.Features.Auth.Constants;
using test.Features.Auth.Contracts;
using test.Features.Auth.DTOs;
using test.Features.Auth.Entities;
using test.Features.Auth.Services;
using test.Infrastructure.Persistence;
using test.Shared.Exceptions;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService
        )
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
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
    private static string NormalizeEmail(string email)
    {
        return email
            .Trim()
            .ToLowerInvariant();
    }
}