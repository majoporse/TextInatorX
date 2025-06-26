using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlobStorage;

public static class BlobStorageInstaller
{
    public static void BlobStorageInstall(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(configuration.GetSection(StorageOptions.OptionsName));
        services.AddSingleton<BlobServiceClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<StorageOptions>>().Value;
            return new BlobServiceClient(options.ConnectionString);
        });
    }
}