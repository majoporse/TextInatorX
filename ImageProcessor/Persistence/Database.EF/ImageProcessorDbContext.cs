using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.EF;

public class ImageProcessorDbContext(DbContextOptions<ImageProcessorDbContext> options)
    : DbContext(options)
{
    public DbSet<ImageText> ImagesWithText { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImageProcessorDbContext).Assembly);
    }
}