using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Infrastructure.Database.Read.Configuration;
public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        //id
        builder.HasKey(x => x.Id);

        //User id
        builder.Property(f => f.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        //fullname
        builder.Property(f => f.FirstName)
            .IsRequired()
            .HasColumnName("first_name");

        builder.Property(f => f.LastName)
            .IsRequired()
            .HasColumnName("last_name");


        //email 
        builder.Property(i => i.Email)
            .IsRequired(false)
            .HasColumnName("email");

        //desc
        builder.Property(d => d.Description)
            .IsRequired()
            .HasColumnName("description");

        //StartVolunteeringDate
        builder.Property(i => i.StartVolunteeringDate)
            .IsRequired()
            .HasColumnName("start_volunteering_date"); 
    }
}
