using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Discussions.Domain;
using PetHome.Core.ValueObjects.PetManagment.Breed;

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
           .HasForeignKey("relation_id");
       
       builder.HasMany(d => d.Messages)
           .WithOne(m => m.Discussion)
           .HasForeignKey("discussion_id")
           .OnDelete(DeleteBehavior.Cascade);
       
       builder.HasMany(d => d.Users)
           .WithMany();
    }
}
