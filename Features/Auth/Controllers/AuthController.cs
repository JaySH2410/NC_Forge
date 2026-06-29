using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test.Features.Auth.Constants;
using test.Features.Auth.Contracts;
using test.Features.Auth.DTOs;
using test.Shared.Entities;
using test.Shared.Responses;

namespace test.Features.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService userService)
    {
        _authService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<RegisterResponse>>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(
            request,
            cancellationToken);

        return Ok(ApiResponse<RegisterResponse>.Success(response, AuthSuccessMessages.RegisterSuccess));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(
    LoginRequest request,
    CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(
            request,
            cancellationToken);

        return Ok(ApiResponse<LoginResponse>.Success(response,AuthSuccessMessages.LoginSuccess));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<CurrentUserResponse>>> GetCurrentUser(
    CancellationToken cancellationToken)
    {
        var response = await _authService.GetCurrentUserAsync(
            cancellationToken);

        return Ok(
            ApiResponse<CurrentUserResponse>.Success(
                response,
                AuthSuccessMessages.CurrentUserFetched));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<RefreshTokenResponse>>> Refresh(
    RefreshTokenRequest request,
    CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshTokenAsync(
            request,
            cancellationToken);

        return Ok(
            ApiResponse<RefreshTokenResponse>.Success(
                response,
                AuthSuccessMessages.TokenRefreshed)
        );
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse>> Logout(
    LogoutRequest request,
    CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request, cancellationToken);

        return Ok(ApiResponse.Success(AuthSuccessMessages.LogoutSuccess));
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse>> ChangePassword(
    ChangePasswordRequest request,
    CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(
            request,
            cancellationToken);

        return Ok(
            ApiResponse.Success(
                AuthSuccessMessages.PasswordChangedSuccessfully));
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse>> ForgotPassword(
    ForgotPasswordRequest request,
    CancellationToken cancellationToken)
    {
        await _authService.ForgotPasswordAsync(
            request,
            cancellationToken);

        return Ok(
            ApiResponse.Success(
                AuthSuccessMessages.PasswordResetEmailSent));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse>> ResetPassword(
    ResetPasswordRequest request,
    CancellationToken cancellationToken)
    {
        await _authService.ResetPasswordAsync(
            request,
            cancellationToken);

        return Ok(
            ApiResponse.Success(
                AuthSuccessMessages.PasswordResetSuccessful));
    }
}