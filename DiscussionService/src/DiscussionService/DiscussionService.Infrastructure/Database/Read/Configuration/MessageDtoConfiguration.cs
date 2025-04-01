using DiscussionService.Application.Database.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscussionService.Infrastructure.Database.Read.Configuration;
public class MessageDtoConfiguration : IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(x => x.Id); 

        builder.Property(m => m.Text) 
            .IsRequired(false)
            .HasColumnName("text");

        builder.Property(m => m.UserId) 
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(m => m.IsEdited)
            .IsRequired()
            .HasColumnName("is_edited");

        builder.Property(m => m.CreatedAt) 
            .IsRequired()
            .HasColumnName("created_at");
         
        builder.Property(m => m.DiscussionId) 
            .IsRequired()
            .HasColumnName("discussion_id");  
    }
}