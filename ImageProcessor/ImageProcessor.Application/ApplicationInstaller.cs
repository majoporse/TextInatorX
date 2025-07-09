using Microsoft.Extensions.DependencyInjection;
using Wolverine.Attributes;

[assembly: WolverineModule]

namespace ImageProcessor.Application;

public static class ApplicationInstaller
{
    public static IServiceCollection AddApplicationInstaller(this IServiceCollection services)
    {
        return services;
    }
}