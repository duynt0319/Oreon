using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Infrastructure.Identity;

namespace Oreon.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ApplicationRole (ASP.NET Core Identity role entity).
/// </summary>
public sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        // Table name
        builder.ToTable("AspNetRoles");

        // Index for performance
        builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

        // Constraints
        builder.Property(r => r.Name).IsRequired().HasMaxLength(256);
    }
}
