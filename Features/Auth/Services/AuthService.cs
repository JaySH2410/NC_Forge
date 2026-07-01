using Microsoft.EntityFrameworkCore;
using test.Features.Auth.Constants;
using test.Features.Auth.Contracts;
using test.Features.Auth.DTOs;
using test.Features.Auth.DTOs.Internal;
using test.Features.Auth.DTOs.Request;
using test.Features.Auth.DTOs.Response;
using test.Features.Auth.Entities;
using test.Features.Auth.Services;
using test.Infrastructure.Persistence;
using test.Shared.Contracts;
using test.Shared.Exceptions;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly ITokenHasher _tokenHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IPasswordResetService _passwordResetService;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        AppDbContext dbContext,
        ITokenHasher tokenHasher,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService,
        IPasswordResetService passwordResetService,
        IEmailVerificationService emailVerificationService,
        ICurrentUserService currentUser,
        ILogger<AuthService> logger
        )
    {
        _dbContext = dbContext;
        _tokenHasher = tokenHasher;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
        _passwordResetService = passwordResetService;
        _emailVerificationService = emailVerificationService;
        _currentUser = currentUser;
        _logger = logger;
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

        var passwordHash =_tokenHasher.Hash(request.Password);

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

        var isValidPassword =_tokenHasher.Verify(request.Password,user.PasswordHash);

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
    public async Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenService.GetValidRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken.UserId != _currentUser.UserId)
        {
            throw new UnauthorizedException(AuthErrorMessages.Unauthorized);
        }

        _refreshTokenService.RevokeRefreshToken(refreshToken, RefreshTokenMessages.UserLoggedOut);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var tokenHash = _refreshTokenService.HashToken(
            request.RefreshToken);

        var refreshToken = await _refreshTokenService.GetValidRefreshTokenAsync(request.RefreshToken, cancellationToken);

        var accessToken = _jwtTokenService.GenerateAccessToken(refreshToken.User);

        var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

        _refreshTokenService.RevokeRefreshToken(refreshToken, RefreshTokenMessages.TokenRotated);
        refreshToken.ReplacedByTokenHash = newRefreshToken.TokenHash;

        var refreshTokenEntity = new RefreshToken
        {
            TokenHash = newRefreshToken.TokenHash,
            ExpiresAt = newRefreshToken.ExpiresAt,
            UserId = refreshToken.UserId
        };

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
    public async Task ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (_currentUser.UserId is null)
        {
            throw new UnauthorizedException(AuthErrorMessages.Unauthorized);
        }

        var user = await _dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(
                x => x.Id == _currentUser.UserId,
                cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(AuthErrorMessages.UserNotFound);
        }

        var isValidPassword = _tokenHasher.Verify(request.CurrentPassword, user.PasswordHash);

        if (!isValidPassword)
        {
            throw new UnauthorizedException(AuthErrorMessages.InvalidCredentials);
        }

        user.PasswordHash = _tokenHasher.Hash(request.NewPassword);

        await _refreshTokenService.RevokeAllUserRefreshTokensAsync(
            user.Id,
            RefreshTokenMessages.PasswordChanged,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);

        // Prevent user enumeration
        if (user is null)
        {
            return;
        }

        var resetToken =
            _passwordResetService.GenerateToken();

        var entity = new PasswordResetToken
        {
            TokenHash = resetToken.TokenHash,
            ExpiresAt = resetToken.ExpiresAt,
            UserId = user.Id
        };

        await _dbContext.PasswordResetTokens.AddAsync(
            entity,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        _logger.LogInformation(
            "Password reset token for {Email}: {Token}",
            user.Email,
            resetToken.Token);
    }
    public async Task ResetPasswordAsync(ResetPasswordRequest request,CancellationToken cancellationToken = default)
    {
        var passwordResetToken =
            await _passwordResetService.GetValidTokenAsync(
                request.Token,
                cancellationToken);

        passwordResetToken.User.PasswordHash =
            _tokenHasher.Hash(request.NewPassword);

        _passwordResetService.MarkAsUsed(
            passwordResetToken);

        await _refreshTokenService.RevokeAllUserRefreshTokensAsync(
            passwordResetToken.UserId,
            RefreshTokenMessages.PasswordChanged,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
    public async Task VerifyEmailAsync(VerifyEmailRequest request, CancellationToken cancellationToken = default)
    {
        var token =
            await _emailVerificationService.GetValidTokenAsync(
                request.Token,
                cancellationToken);

        token.User.IsEmailVerified = true;

        _emailVerificationService.MarkAsUsed(
            token);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
    public async Task ResendVerificationEmailAsync(CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(
                x => x.Id == _currentUser.UserId,
                cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                AuthErrorMessages.UserNotFound);
        }

        if (user.IsEmailVerified)
        {
            throw new BusinessException(
                AuthErrorMessages.EmailAlreadyVerified);
        }

        var verificationToken =
            _emailVerificationService.GenerateToken();

        var entity = new EmailVerificationToken
        {
            TokenHash = verificationToken.TokenHash,
            ExpiresAt = verificationToken.ExpiresAt,
            UserId = user.Id
        };

        await _dbContext.EmailVerificationTokens.AddAsync(
            entity,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        _logger.LogInformation(
            "Email verification token for {Email}: {Token}",
            user.Email,
            verificationToken.Token);
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
    private static string NormalizeEmail(string email)
    {
        return email
            .Trim()
            .ToLowerInvariant();
    }
}