using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database.EF;

namespace Persistence;

public class MigrateDatabaseOnStartup(IServiceScopeFactory scopeFactory) : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ImageProcessorDbContext>();
        context.Database.EnsureCreated();
        return next;
    }
}