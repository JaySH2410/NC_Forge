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
    public IActionResult Me()
    {
        return Ok("Authenticated");
    }
}