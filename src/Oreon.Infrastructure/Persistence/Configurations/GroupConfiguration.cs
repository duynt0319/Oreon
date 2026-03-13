using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Presence;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(g => g.Name);

        builder.HasMany(g => g.Connections)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(g => g.Connections).HasField("_connections");
    }
}
