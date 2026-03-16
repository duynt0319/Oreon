using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Members;
using Oreon.Infrastructure.Identity;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(m => m.Id);

        builder
            .Property(m => m.Id)
            .HasConversion(id => id.Value, value => MemberId.Of(value))
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();

        builder.Property(m => m.AppUserId).HasColumnType("uniqueidentifier").IsRequired();

        builder.HasIndex(m => m.AppUserId).IsUnique();

        builder
            .HasOne<AppUser>()
            .WithOne()
            .HasForeignKey<Member>(m => m.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.KnownAs).IsRequired().HasMaxLength(100);

        builder.Property(m => m.Gender).IsRequired().HasMaxLength(20);

        builder.Property(m => m.City).IsRequired().HasMaxLength(100);

        builder.Property(m => m.Country).IsRequired().HasMaxLength(100);

        builder.Property(m => m.Introduction).HasMaxLength(2000);

        builder.Property(m => m.LookingFor).HasMaxLength(2000);

        builder.Property(m => m.Interests).HasMaxLength(1000);

        builder.Property(m => m.DateOfBirth).IsRequired();

        builder.Property(m => m.Created).IsRequired();

        builder.Property(m => m.LastActive).IsRequired();

        builder.Ignore(m => m.DomainEvents);

        // One Member has many Photos (1:N relationship)
        builder
            .HasMany(m => m.Photos)
            .WithOne()
            .HasForeignKey(p => p.MemberId)
            .HasPrincipalKey(m => m.Id)
            .HasConstraintName("FK_Photos_Members_MemberId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(m => m.Photos).HasField("_photos");
    }
}
