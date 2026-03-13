using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oreon.Domain.Aggregates.Messages;

namespace Oreon.Infrastructure.Persistence.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => MessageId.Of(value))
            .ValueGeneratedNever();

        builder.Property(m => m.SenderUsername)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("SenderUserName");

        builder.Property(m => m.RecipientUsername)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("RecipientUserName");

        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.SenderId)
            .IsRequired();

        builder.Property(m => m.RecipientId)
            .IsRequired();

        builder.Property(m => m.MessageSent)
            .IsRequired();

        builder.Property(m => m.SenderDeleted)
            .IsRequired();

        builder.Property(m => m.RecipientDeleted)
            .IsRequired();

        builder.HasIndex(m => m.SenderId);
        builder.HasIndex(m => m.RecipientId);
        builder.HasIndex(m => m.MessageSent);

        // Ignore domain events (not persisted)
        builder.Ignore(m => m.DomainEvents);
    }
}
