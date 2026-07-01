using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using test.Features.Auth.Contracts;
using test.Features.Auth.Services;
using test.Infrastructure.Configuration;
using test.Shared.Contracts;
using test.Shared.Services;

namespace test.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        // DbContext
        // Options
        //Shared
        ////User
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        ////Security
        services.AddSingleton<ITokenHasher, TokenHasher>();
        services.AddSingleton<ISecureTokenGenerator, SecureTokenGenerator>();
        // Services
        ////Authentication
        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IEmailVerificationService, EmailVerificationService>();
        services.AddScoped<IAuthService, AuthService>();
        // JWT
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));  
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtOptions = configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>()!;

            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Secret)),

                    ClockSkew = TimeSpan.Zero
                };
        });
        services.AddAuthorization();


        return services;
    }
}