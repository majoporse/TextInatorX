using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Interfaces;

public interface IPersistenceInstall
{
    public void Install(IServiceCollection services, IConfiguration configuration);
}