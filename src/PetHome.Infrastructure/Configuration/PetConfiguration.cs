using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Domain.PetEntity;
using PetHome.Domain.Shared;

namespace PetHome.Infrastructure.Configuration
{
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
            builder.ComplexProperty(n => n.Name, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("name");
            });

            //speacies
            builder.ComplexProperty(s => s.SpeciesId, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("species_id");
            });

            //desc
            builder.Property(d => d.Description)
                .HasMaxLength(Constants.MAX_DESC_LENGHT)
                .IsRequired()
                .HasColumnName("description");

            //breed
            builder.ComplexProperty(b => b.BreedId, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("breed_id");
            });

            //color
            builder.ComplexProperty(c => c.Color, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("color");
            });

            //shelter
            builder.ComplexProperty(s => s.ShelterId, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("shelter_id");
            });

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
            builder.ComplexProperty(d => d.BirthDate, tb =>
            {
                tb.Property(v => v.Value)
                .HasConversion(
                    d => d.ToShortDateString(),
                    d => DateOnly.Parse(d))
                .IsRequired()
                .HasColumnName("color");
            });

            //is vaccinated
            builder.Property(c => c.IsVaccinated)
                .IsRequired()
                .HasColumnName("is_vaccinated");

            //status
            builder.Property(s => s.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasColumnName("status");

            //requisites
            builder.ComplexProperty(req => req.Requisites, tb =>
            {
                tb.Property(n => n.Name)
                .IsRequired()
                .HasColumnName("requisit_name");

                tb.Property(d => d.Description)
                .HasMaxLength(Constants.MAX_DESC_LENGHT)
                .IsRequired()
                .HasColumnName("requisit_description");

                tb.Property(m => m.PaymentMethod)
                .HasConversion<string>()
                .IsRequired()
                .HasColumnName("requisit_payment_method");
            });

            //create date
            builder.ComplexProperty(d => d.ProfileCreateDate, tb =>
            {
                tb.Property(v => v.Value)
                .HasConversion(
                    d => d.ToShortDateString(),
                    d => DateOnly.Parse(d))
                .IsRequired()
                .HasColumnName("profile_create_date");
            });

            //volunteerid
            builder.ComplexProperty(v => v.VolunteerId, tb =>
            {
                tb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("volunteer_id");
            });

        }
    }
}
