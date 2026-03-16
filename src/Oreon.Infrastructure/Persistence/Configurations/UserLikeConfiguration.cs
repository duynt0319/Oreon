using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Likes;
using Oreon.Domain.Aggregates.Members;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
{
    public void Configure(EntityTypeBuilder<UserLike> builder)
    {
        builder.ToTable("UserLikes");

        builder.HasKey(k => new { k.SourceMemberId, k.TargetMemberId });

        builder
            .Property(ul => ul.SourceMemberId)
            .HasConversion(id => id.Value, value => MemberId.Of(value))
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder
            .Property(ul => ul.TargetMemberId)
            .HasConversion(id => id.Value, value => MemberId.Of(value))
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.HasIndex(ul => ul.SourceMemberId);
        builder.HasIndex(ul => ul.TargetMemberId);

        // Foreign keys to Members aggregate (1:N relationships)
        // One Member can give many Likes
        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(ul => ul.SourceMemberId)
            .HasPrincipalKey(member => member.Id)
            .HasConstraintName("FK_UserLikes_Members_SourceMemberId")
            .OnDelete(DeleteBehavior.Restrict);

        // One Member can receive many Likes
        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(ul => ul.TargetMemberId)
            .HasPrincipalKey(member => member.Id)
            .HasConstraintName("FK_UserLikes_Members_TargetMemberId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
