using PetHome.SharedKernel.ValueObjects.RolePermission;

namespace AccountService.Domain.Aggregates;
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

    public static RolePermission Create(
        Role role,
        Permission permission)
    {
        RoleId roleId = RoleId.Create(role.Id).Value;

        return new RolePermission(roleId, permission.Id);
    }

    public static IReadOnlyList<RolePermission> Create(IEnumerable<Role> roles)
    {
        var result = new List<RolePermission>();
        foreach (var role in roles)
        {
            foreach (var permission in role.Permissions)
            {
                var rolePermission = RolePermission.Create(role, permission);
                result.Add(rolePermission);
            }
        }
        return result;
    }
}
