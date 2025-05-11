using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.VolunteerRequests.Application.Database.Dto;

namespace PetHome.VolunteerRequests.Infrastructure.Database.Read.Configuration;
public class VolunteerRequestDtoConfiguration : IEntityTypeConfiguration<VolunteerRequestDto>
{
    public void Configure(EntityTypeBuilder<VolunteerRequestDto> builder)
    {
        builder.ToTable("VolunteerRequests");

        builder.HasKey(v => v.Id); 

        builder.Property(v => v.AdminId) 
            .IsRequired(false)
            .HasColumnName("admin_id");

        builder.Property(v => v.UserId) 
            .IsRequired(false)
            .HasColumnName("user_id");

        builder.Property(v => v.VolunteerInfo) 
            .IsRequired(false)
            .HasColumnName("volunteer_info");

        builder.Property(s => s.Status) 
            .IsRequired(false)
            .HasColumnName("request_status");

        builder.Property(v => v.CreatedAt) 
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(v => v.RejectedComment) 
            .IsRequired()
            .HasColumnName("rejected_comment");

        builder.Property(v => v.DiscussionId) 
            .IsRequired()
            .HasColumnName("discussion_id");
    }
}
