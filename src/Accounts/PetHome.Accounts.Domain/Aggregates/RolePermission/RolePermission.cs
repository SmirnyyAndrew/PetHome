namespace PetHome.Accounts.Domain.Aggregates.RolePermission;
public class RolePermission
{
    public RoleId? RoleId { get; private set; }    
    public PermissionId? PermissionId { get; private set; } 


    private RolePermission() { }
    private RolePermission(RoleId roleId, PermissionId permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public static RolePermission Create(
        RoleId roleId,
        PermissionId permissionId)
    {
        return new RolePermission(roleId, permissionId);
    }
}
