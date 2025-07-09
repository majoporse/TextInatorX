namespace Domain.Entities;

public class ImageText
{
    public Guid Id { get; set; }
    public Guid ImageId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}