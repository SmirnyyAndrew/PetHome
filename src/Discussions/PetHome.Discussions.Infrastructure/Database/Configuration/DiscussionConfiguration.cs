using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Discussions.Domain;

namespace PetHome.Discussions.Infrastructure.Database.Configuration;
public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable("discussions");

        builder.HasKey(x => x.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                 id => id.Value,
                 value => DiscussionId.Create(value).Value)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(v => v.RelationId)
            .HasConversion(
                 id => id.Value,
                 value => RelationId.Create(value).Value)
            .IsRequired()
            .HasColumnName("relation_id");

        builder.Property(d => d.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasColumnName("status");

        builder.HasOne(d => d.Relation)
            .WithMany(r => r.Discussions)
           .HasForeignKey(d => d.RelationId);

        builder.HasMany(d => d.Messages)
            .WithOne(m => m.Discussion)
            .HasForeignKey(m => m.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Users)
            .WithMany();
    }
}
