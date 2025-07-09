using ImageProcessor.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Database.EF;

namespace Persistence;

public static class PersistenceInstaller
{
    public static IServiceCollection PersistenceInstall(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PersistenceOptions>(configuration.GetSection(PersistenceOptions.OptionsName));
        services.AddDbContext<ImageProcessorDbContext>((provider, builder) =>
        {
            var options = provider.GetRequiredService<IOptions<PersistenceOptions>>();
            Console.WriteLine($"Using MongoDB connection string: {options.Value.ConnectionString}");
            builder.UseMongoDB(options.Value.ConnectionString, options.Value.DatabaseName);
        });

        services.AddScoped<IImageTextRepository, ImageProcessorRepository>();
        services.AddSingleton<IStartupFilter, MigrateDatabaseOnStartup>();

        return services;
    }
}