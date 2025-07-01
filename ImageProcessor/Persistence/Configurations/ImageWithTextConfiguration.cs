using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ImageWithTextConfiguration : IEntityTypeConfiguration<ImageText>
{
    public void Configure(EntityTypeBuilder<ImageText> builder)
    {
        builder.HasKey(e => e.Id);
    }
}