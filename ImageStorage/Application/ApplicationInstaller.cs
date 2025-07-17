using Microsoft.Extensions.DependencyInjection;
using Wolverine.Attributes;

[assembly: WolverineModule]

namespace Application;

public static class ApplicationInstaller
{
    public static void ApplicationInstall(this IServiceCollection serviceCollection)
    {
        //just a place for wolverine assembly tag
    }
}