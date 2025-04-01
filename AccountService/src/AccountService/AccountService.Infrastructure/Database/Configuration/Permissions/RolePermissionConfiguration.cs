using AccountService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHome.Core.ValueObjects.RolePermission;

namespace AccountService.Infrastructure.Database.Configuration.Permissions;
public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permission");

        builder.HasKey(i => new { i.RoleId, i.PermissionId });  
        
        builder.Property(r => r.RoleId)
            .HasConversion(
                i => i.Value,
                value => RoleId.Create(value).Value)
            .IsRequired()
            .HasColumnName("role_id");

        builder.Property(r => r.PermissionId)
            .HasConversion(
                i => i.Value,
                value => PermissionId.Create(value).Value)
            .IsRequired()
            .HasColumnName("permission_id"); 
    }
}
