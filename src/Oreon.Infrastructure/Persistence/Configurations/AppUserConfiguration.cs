using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Infrastructure.Identity;

namespace Oreon.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ApplicationUser (ASP.NET Core Identity user entity).
/// This configuration is in Infrastructure layer since Identity is a technical concern.
/// </summary>
public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        // Table name (default is AspNetUsers, but can customize)
        builder.ToTable("AspNetUsers");

        // Additional indexes for performance
        builder.HasIndex(u => u.NormalizedUserName)
            .HasDatabaseName("UserNameIndex")
            .IsUnique();

        builder.HasIndex(u => u.NormalizedEmail)
            .HasDatabaseName("EmailIndex");

        // Constraints
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Email)
            .HasMaxLength(256);

        // If you need to add custom properties to ApplicationUser in the future:
        // builder.Property(u => u.CustomProperty)
        //     .HasMaxLength(100);
    }
}
