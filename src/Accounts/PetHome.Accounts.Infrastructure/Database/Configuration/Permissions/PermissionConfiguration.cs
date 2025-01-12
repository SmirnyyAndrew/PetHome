using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Core.ValueObjects.RolePermission;

namespace PetHome.Accounts.Infrastructure.Database.Configuration.Permissions;
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasConversion(
            i => i.Value,
            value => PermissionId.Create(value).Value);

        builder.Property(i => i.Code)
            .HasConversion(
                name => name.Value,
                value => PermissionCode.Create(value).Value)
            .IsRequired()
            .HasColumnName("code"); 
    }
}
