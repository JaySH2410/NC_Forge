using test.Features.Auth.Contracts;
using test.Features.Auth.Services;

namespace test.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        // JWT
        // Options
        // Services
        services.AddScoped<IPasswordHasher, PasswordHasherService>();

        return services;
    }
}