using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database.EF;

namespace Persistence;

public class DatabaseMigrator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DatabaseMigrator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Migrate()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
    }
}