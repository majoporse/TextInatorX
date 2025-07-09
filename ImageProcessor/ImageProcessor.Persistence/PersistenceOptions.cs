namespace Persistence;

public class PersistenceOptions
{
    public const string OptionsName = "ImageProcessor.Persistence";
    public string DatabaseName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}