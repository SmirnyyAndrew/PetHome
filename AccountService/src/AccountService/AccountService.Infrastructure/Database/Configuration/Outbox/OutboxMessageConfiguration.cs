using AccountService.Domain.Others;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Database.Configuration.Outbox;
public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Type)
            .IsRequired();

        builder.Property(o => o.Payload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(o => o.OccurredOn)
            .HasConversion(v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired()
            .HasColumnName("occurred_on");

        builder.Property(o => o.ProcessedOn)
            .HasConversion(v => v.Value.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired(false)
            .HasColumnName("processed_on");

        builder.Property(o => o.Error)
            .IsRequired(false)
            .HasColumnName("error");

        builder.HasIndex(e => new
        {
            e.OccurredOn,
            e.ProcessedOn
        })
            .HasDatabaseName("idx_outbox_messages_unprocessed")
            .IncludeProperties(p => new
            {
                p.Id,
                p.Type,
                p.Payload,
            })
            .HasFilter("processed_on IS NULL");
    }
}
