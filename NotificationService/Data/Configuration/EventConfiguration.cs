using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Features.Broker;

namespace NotificationService.Data.Configuration;

public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable("events");
        builder.HasKey(x => x.EventId);

        builder.Property(x => x.EventId)
            .HasColumnName("event_id")
            .IsRequired();
        builder.Property(x => x.ProcessedAt)
            .HasColumnName("processed_at")
            .IsRequired();
        builder.Property(x => x.Handler)
            .HasColumnName("handler")
            .IsRequired()
            .HasMaxLength(100);
    }
}