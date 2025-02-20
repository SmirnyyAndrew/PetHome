namespace PetHome.Framework.Auth;
public class UserScopedData
{
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid RoleId { get; set; } = Guid.Empty;
    public IReadOnlyList<Guid> PermissionsIds { get; set; } = [];
    public string UserName { get; set; } = string.Empty;
}
 