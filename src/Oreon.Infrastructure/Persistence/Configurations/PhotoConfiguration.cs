using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Members;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, value => PhotoId.Of(value))
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();

        builder.Property(p => p.Url).IsRequired().HasMaxLength(500);

        builder.Property(p => p.PublicId).HasMaxLength(200);

        builder.Property(p => p.IsMain).IsRequired();

        builder
            .Property(p => p.MemberId)
            .HasConversion(memberId => memberId.Value, value => MemberId.Of(value))
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.HasIndex(p => p.MemberId);

        // Foreign key to Members aggregate (configured from Member side, but explicit here for clarity)
        // The actual FK relationship is defined in MemberConfiguration with CASCADE delete
    }
}
