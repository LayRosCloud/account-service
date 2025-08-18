using AccountService.Broker.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Utils.Data.Configuration;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.EventId);

        builder.Property(e => e.EventId)
            .IsRequired()
            .HasColumnName("event_id");
        builder.Property(e => e.ProcessedAt)
            .IsRequired()
            .HasColumnName("processed_at");

        builder.Property(e => e.Handler)
            .IsRequired()
            .HasColumnName("handler");
    }
}