using FilesService.Core.Dto.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Volunteers.Application.Database.Dto;
using System.Text.Json;

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

        //avatar url
        builder.Ignore(x => x.AvatarUrl);

        //avatar
        builder.ComplexProperty(a => a.Avatar, ab =>
        {
            ab.Property(p => p.Type)
            .HasConversion<string>()
            .HasColumnName("type");

            ab.Property(p => p.Key)
            .HasColumnName("key");

            ab.Property(p => p.BucketName)
            .HasColumnName("bucket_name");

            ab.Property(p => p.FileName)
            .HasColumnName("file_name");
        });

        //photos urls
        builder.Ignore(p=>p.PhotosUrls);

        //photos
        builder.Property(s => s.Photos)
          .HasConversion(
               u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
               json => JsonSerializer.Deserialize<IReadOnlyList<MediaFile>>(json, JsonSerializerOptions.Default))
          .HasColumnName("photos");
         
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
