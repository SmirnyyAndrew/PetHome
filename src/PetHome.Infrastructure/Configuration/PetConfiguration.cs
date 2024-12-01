using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetEntity;
using PetHome.Domain.Shared;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Infrastructure.Configuration;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        //id
        builder.HasKey(x => x.Id);
        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value))
            .IsRequired()
            .HasColumnName("id");

        //name
        builder.Property(i => i.Name)
            .HasConversion(
                name => name.Value,
                value => PetName.Create(value).Value)
            .IsRequired()
            .HasColumnName("name");
         
        //speacies 
        builder.Property(i => i.SpeciesId)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value).Value)
            .IsRequired()
            .HasColumnName("species_id");

        //desc
        builder.Property(d => d.Description)
            .HasMaxLength(Constants.MAX_DESC_LENGHT)
            .IsRequired()
            .HasColumnName("description");

        //breed
        builder.Property(i => i.BreedId)
            .HasConversion(
                b => b.Value,
                value => BreedId.Create(value))
            .IsRequired(false)
            .HasColumnName("breed_id");

        //color 
        builder.Property(i => i.Color)
            .HasConversion(
                с => с.Value,
                value => Color.Create(value).Value)
            .IsRequired()
            .HasColumnName("color");

        //shelter id
        builder.Property(i => i.ShelterId)
            .HasConversion(
                id => id.Value,
                value => PetShelterId.Create(value))
            .IsRequired()
            .HasColumnName("shelter_id");

        //weight
        builder.Property(w => w.Weight)
            .HasMaxLength(Constants.MAX_WEIGHT)
            .IsRequired()
            .HasColumnName("weight");

        //is castrated
        builder.Property(c => c.IsCastrated)
            .IsRequired()
            .HasColumnName("is_castrated");

        //bith date
        builder.Property(i => i.BirthDate)
            .HasConversion(
                d => d.Value,
                value => Date.Create(value).Value)
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

        ////requisites
        builder.OwnsOne(r => r.RequisitesDetails, d =>
        {
            d.ToJson();
            d.OwnsMany(d => d.Values);
        });
         
        //create date
        builder.Property(i => i.ProfileCreateDate)
            .HasConversion(
                d => d.Value,
                value => Date.Create(value).Value)
            .IsRequired()
            .HasColumnName("profile_create_date");
         
        //volunteer id
        builder.Property(i => i.VolunteerId)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value))
            .IsRequired()
            .HasColumnName("volunteer_id");
    }
}
