using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Volunteers.Infrastructure.Database.Write.Configuration;

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
                value => PetId.Create(value).Value)
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
            .HasConversion(
            desc => desc.Value,
            value => Description.Create(value).Value)
            .IsRequired()
            .HasColumnName("description");

        //breed
        builder.Property(i => i.BreedId)
            .HasConversion(
                b => b.Value,
                value => BreedId.Create(value).Value)
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
                value => PetShelterId.Create(value).Value)
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
        builder.OwnsOne(r => r.Requisites, d =>
        {
            d.ToJson("requisites");
            d.OwnsMany(d => d.Values, rb =>
            {
                rb.Property(r => r.Name)
                .IsRequired();

                rb.Property(r => r.Description)
                .IsRequired();

                rb.Property(r => r.PaymentMethod)
                .IsRequired();
            });
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
                value => VolunteerId.Create(value).Value)
            .IsRequired()
            .HasColumnName("volunteer_id");

        //is soft deleted
        builder.Property(d=>d.IsDeleted)  
            .HasColumnName("is_deleted");

        //has been soft deleted date
        builder.Property(d=>d.DeletionDate)  
            .HasColumnName("soft_deleted_date");

        //serial number
        builder.Property(s => s.SerialNumber)
            .HasConversion(
                num => num.Value,
                value => SerialNumber.Create(value))
            .IsRequired()
            .HasColumnName("serial_number");

        //photos
        builder.OwnsOne(d => d.Medias, db =>
        {
            db.ToJson("photos");
            db.OwnsMany(db => db.Values, pb =>
            {
                pb.Property(p => p.BucketName)
                .IsRequired();

                pb.Property(p => p.FileName)
                .IsRequired();
            });
        });
    }
}
