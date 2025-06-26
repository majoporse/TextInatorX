using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Interfaces;

public interface IStorageInstall
{
    public void Install(IServiceCollection services, IConfiguration configuration);
}