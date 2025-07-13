namespace ImageProcessor.Application;

public class TesseractOptions
{
    public const string OptionsName = "Tesseract";
    public string Language { get; set; } = string.Empty;
    public string DataPath { get; set; } = string.Empty;
}