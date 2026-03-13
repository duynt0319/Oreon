using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Members;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("photos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PhotoId.Of(value))
            .ValueGeneratedNever();

        builder.Property(p => p.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.PublicId)
            .HasMaxLength(200);

        builder.Property(p => p.IsMain)
            .IsRequired();

        builder.Property(p => p.AppUserId)
            .IsRequired();

        builder.HasIndex(p => p.AppUserId);
    }
}
