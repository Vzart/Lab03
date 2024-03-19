using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Persistence;
using Repository.Shared;

namespace Repository;

public static class ConfigureServices
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationDbContext(configuration);
        services.AddScoped<IApplicationDbContext>(sp => sp.GetService<ApplicationDbContext>()!);

        return services;
    }
    
    private static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.UseSqlServer(databaseSettings.ConnectionString!, options =>
            {
                options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });
        });

        return services;
    }
}