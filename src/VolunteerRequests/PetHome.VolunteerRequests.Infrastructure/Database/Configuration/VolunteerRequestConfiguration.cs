using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Infrastructure.Database.Configuration;
public class VolunteerRequestConfiguration : IEntityTypeConfiguration<VolunteerRequest>
{
    public void Configure(EntityTypeBuilder<VolunteerRequest> builder)
    {
        builder.ToTable("VolunteerRequests");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerRequestId.Create(value).Value)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(v => v.AdminId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value).Value)
            .IsRequired(false)
            .HasColumnName("admin_id");

        builder.Property(v => v.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value).Value)
            .IsRequired(false)
            .HasColumnName("user_id");

        builder.Property(v => v.VolunteerInfo)
            .HasConversion(
                i => i.Value,
                value => VolunteerInfo.Create(value).Value)
            .IsRequired(false)
            .HasColumnName("volunteer_info");

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .IsRequired(false)
            .HasColumnName("request_status");

        builder.Property(v => v.CreatedAt)
            .HasConversion(
                d => d.Value,
                value => Date.Create(value).Value)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(v => v.RejectedComment)
            .HasConversion(
                c => c.Value,
                value => RequestComment.Create(value).Value)
            .IsRequired()
            .HasColumnName("rejected_comment");

        builder.Property(v => v.DiscussionId)
            .HasConversion(
                id => id.Value,
                value => DiscussionId.Create(value).Value)
            .IsRequired()
            .HasColumnName("discussion_id");
    } 
}
