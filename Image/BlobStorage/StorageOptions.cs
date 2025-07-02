namespace BlobStorage;

public class StorageOptions
{
    public const string OptionsName = "BlobStorage";
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}