using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Members;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("members");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => MemberId.Of(value))
            .ValueGeneratedNever();

        builder.Property(m => m.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(m => m.Username)
            .IsUnique();

        builder.Property(m => m.KnownAs)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Gender)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.City)
            .HasMaxLength(100);

        builder.Property(m => m.Country)
            .HasMaxLength(100);

        builder.Property(m => m.Introduction)
            .HasMaxLength(2000);

        builder.Property(m => m.LookingFor)
            .HasMaxLength(2000);

        builder.Property(m => m.Interests)
            .HasMaxLength(1000);

        builder.Property(m => m.DateOfBirth)
            .IsRequired();

        builder.Property(m => m.Created)
            .IsRequired();

        builder.Property(m => m.LastActive)
            .IsRequired();

        // Ignore domain events (not persisted)
        builder.Ignore(m => m.DomainEvents);

        // Photos collection
        builder.HasMany(m => m.Photos)
            .WithOne()
            .HasForeignKey(p => p.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Enable field access for private collection
        builder.Navigation(m => m.Photos).HasField("_photos");
    }
}
