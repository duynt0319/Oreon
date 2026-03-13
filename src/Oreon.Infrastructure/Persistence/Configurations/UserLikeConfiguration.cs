using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Likes;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
{
    public void Configure(EntityTypeBuilder<UserLike> builder)
    {
        builder.ToTable("user_likes");

        builder.HasKey(k => new { k.SourceUserId, k.TargetUserId });

        builder.Property(ul => ul.SourceUserId)
            .IsRequired();

        builder.Property(ul => ul.TargetUserId)
            .IsRequired();

        builder.HasIndex(ul => ul.SourceUserId);
        builder.HasIndex(ul => ul.TargetUserId);
    }
}
