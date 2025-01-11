using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Volunteers.Application.Database.Dto;

namespace PetHome.Volunteers.Infrastructure.Database.Read.Configuration;
public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");
        //id
        builder.HasKey(x => x.Id);

        //name
        builder.Property(i => i.Name)
            .IsRequired()
            .HasColumnName("name");

        //speacies 
        builder.Property(i => i.SpeciesId)
            .IsRequired()
            .HasColumnName("species_id");

        //desc
        builder.Property(d => d.Description)
            .IsRequired()
            .HasColumnName("description");

        //breed
        builder.Property(i => i.BreedId)
            .IsRequired(false)
            .HasColumnName("breed_id");

        //color 
        builder.Property(i => i.Color)
            .IsRequired()
            .HasColumnName("color");

        //shelter id
        builder.Property(i => i.ShelterId)
            .IsRequired()
            .HasColumnName("shelter_id");

        //weight
        builder.Property(w => w.Weight)
            .IsRequired()
            .HasColumnName("weight");

        //is castrated
        builder.Property(c => c.IsCastrated)
            .IsRequired()
            .HasColumnName("is_castrated");

        //bith date
        builder.Property(i => i.BirthDate)
            .IsRequired(false)
            .HasColumnName("bith_date");

        //is vaccinated
        builder.Property(c => c.IsVaccinated)
            .IsRequired()
            .HasColumnName("is_vaccinated");

        //status
        builder.Property(s => s.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasColumnName("status");

        //create date
        builder.Property(i => i.ProfileCreateDate)
            .IsRequired()
            .HasColumnName("profile_create_date");

        //volunteer id
        builder.Property(i => i.VolunteerId)
            .IsRequired()
            .HasColumnName("volunteer_id"); 

        //serial number
        builder.Property(s => s.SerialNumber)
            .IsRequired()
            .HasColumnName("serial_number");  
    }
}
