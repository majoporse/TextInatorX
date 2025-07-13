namespace Domain.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}