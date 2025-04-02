using AccountService.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Infrastructure.Database.Configuration.Users;
public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");

        builder.HasKey(i => i.UserId);
        builder.Property(i => i.UserId)
            .HasConversion(
                i => i.Value,
                value => UserId.Create(value).Value)
            .HasColumnName("user_id");

        builder.Property(d => d.IsDeleted)
            .HasColumnName("is_deleted");

        builder.Property(d => d.DeletionDate)
            .HasColumnName("soft_deleted_date");
    }
}
