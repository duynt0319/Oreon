using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oreon.Domain.Aggregates.Likes;
using Oreon.Domain.Aggregates.Members;
using Oreon.Domain.Aggregates.Messages;
using Oreon.Domain.Aggregates.Presence;
using Oreon.Infrastructure.Identity;

namespace Oreon.Infrastructure.Persistence;

public class DataContext
    : IdentityDbContext<
        AppUser,
        AppRole,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>
    >
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>().HaveColumnType("date");

        base.ConfigureConventions(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
