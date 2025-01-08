using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Domain.Aggregates.User.Accounts;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using System.Text.Json;

namespace PetHome.Accounts.Infrastructure.Database.Configuration.Users;
public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");

        builder.HasKey(i => i.UserId);
        builder.Property(i => i.UserId)
            .HasConversion(
                i => i.Value,
                value => UserId.Create(value).Value)
            .HasColumnName("user_id");

        builder.Property(i => i.FavoritePets)
            .HasConversion(
                 u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                 json => JsonSerializer.Deserialize<IReadOnlyList<Pet>>(json, JsonSerializerOptions.Default))
            .HasColumnName("favorite_pets");

        builder.Property(d => d.IsDeleted)
            .IsRequired(false)
            .HasColumnName("is_deleted");

        builder.Property(d => d.DeletionDate)
            .IsRequired(false)
            .HasColumnName("soft_deleted_date");
    }
}
