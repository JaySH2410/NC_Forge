using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using Forge.Features.Auth.Contracts;
using Forge.Features.Auth.Services;
using Forge.Infrastructure.Configuration;
using Forge.Shared.Contracts;
using Forge.Shared.Services;

namespace Forge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        // DbContext
        services.Configure<DatabaseSettings>(configuration.GetSection("Database"));
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
        ////MetaSchema
        services.AddScoped<IMetaSchemaService, MetaSchemaService>();
        services.AddScoped<IGraphTraversalService, GraphTraversalService>();
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