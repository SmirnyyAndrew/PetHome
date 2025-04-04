using FilesService.Core.Dto.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetManagementService.Application.Database.Dto;
using System.Text.Json;

namespace PetManagementService.Infrastructure.Database.Read.Configuration;
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
        builder.Ignore(r => r.Avatar);
        //builder.OwnsOne(d => d.Avatar, db =>
        //{
        //    db.ToJson("avatar");

        //    db.Property(p => p.Key)
        //    .IsRequired(false)
        //    .HasColumnName("key");

        //    db.Property(p => p.BucketName)
        //        .IsRequired(false)
        //    .HasColumnName("bucket_name");

        //    db.Property(p => p.Type)
        //    .HasConversion<string>()
        //     .IsRequired(false)
        //    .HasColumnName("type");

        //    db.Property(p => p.FileName)
        //    .IsRequired(false)
        //    .HasColumnName("file_name");
        //});

        //photos urls
        //TODO:
        builder.Ignore(p=>p.PhotosUrls);

        //photos
        //TODO:
        builder.Ignore(p => p.Photos);
        //builder.Property(s => s.Photos)
        //  .HasConversion(
        //       u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
        //       json => JsonSerializer.Deserialize<IReadOnlyList<MediaFile>>(json, JsonSerializerOptions.Default))
        //  .HasColumnName("photos");
         
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
