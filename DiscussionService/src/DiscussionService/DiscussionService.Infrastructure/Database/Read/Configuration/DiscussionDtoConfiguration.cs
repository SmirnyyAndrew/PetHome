using DiscussionService.Application.Database.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscussionService.Infrastructure.Database.Read.Configuration;
public class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
    {
        builder.ToTable("discussions");

        builder.HasKey(x => x.Id);   

        builder.Property(v => v.RelationId) 
            .IsRequired()
            .HasColumnName("relation_id");

        builder.Property(d => d.Status)
            .IsRequired()
            .HasColumnName("status");

        builder.HasOne(d => d.Relation)
            .WithMany(r => r.Discussions)
            .HasForeignKey(d => d.RelationId);

        builder.HasMany(d => d.Messages)
            .WithOne(m => m.Discussion)
            .HasForeignKey(m => m.DiscussionId); 
    }
}
