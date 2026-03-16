using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Members;
using Oreon.Domain.Aggregates.Messages;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(m => m.Id);

        builder
            .Property(m => m.Id)
            .HasConversion(id => id.Value, value => MessageId.Of(value))
            .ValueGeneratedNever();

        builder
            .Property(m => m.SenderUsername)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("SenderUserName");

        builder
            .Property(m => m.RecipientUsername)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("RecipientUserName");

        builder.Property(m => m.Content).IsRequired().HasMaxLength(2000);

        builder
            .Property(m => m.SenderMemberId)
            .HasConversion(id => id.Value, value => MemberId.Of(value))
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder
            .Property(m => m.RecipientMemberId)
            .HasConversion(id => id.Value, value => MemberId.Of(value))
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(m => m.MessageSent).IsRequired();

        builder.Property(m => m.SenderDeleted).IsRequired();

        builder.Property(m => m.RecipientDeleted).IsRequired();

        builder.HasIndex(m => m.SenderMemberId);
        builder.HasIndex(m => m.RecipientMemberId);
        builder.HasIndex(m => m.MessageSent);

        // Foreign keys to Members aggregate (1:N relationships)
        // One Member can send many Messages
        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(m => m.SenderMemberId)
            .HasPrincipalKey(member => member.Id)
            .HasConstraintName("FK_Messages_Members_SenderMemberId")
            .OnDelete(DeleteBehavior.Restrict);

        // One Member can receive many Messages
        builder
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(m => m.RecipientMemberId)
            .HasPrincipalKey(member => member.Id)
            .HasConstraintName("FK_Messages_Members_RecipientMemberId")
            .OnDelete(DeleteBehavior.Restrict);

        // Ignore domain events (not persisted)
        builder.Ignore(m => m.DomainEvents);
    }
}
