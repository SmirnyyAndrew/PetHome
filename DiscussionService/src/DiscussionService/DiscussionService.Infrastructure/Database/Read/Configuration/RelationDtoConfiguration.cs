using DiscussionService.Application.Database.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscussionService.Infrastructure.Database.Read.Configuration;
public class RelationDtoConfiguration : IEntityTypeConfiguration<RelationDto>
{
    public void Configure(EntityTypeBuilder<RelationDto> builder)
    {
        builder.ToTable("relations");

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id) 
            .IsRequired()
            .HasColumnName("id");

        builder.Property(r => r.Name) 
            .IsRequired()
            .HasColumnName("name");
    }
}
