using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Discussions.Application.Database.Dto;

namespace PetHome.Discussions.Infrastructure.Database.Read.Configuration;
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
