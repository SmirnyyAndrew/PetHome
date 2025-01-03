using Microsoft.AspNetCore.Authorization;

namespace PetHome.Accounts.Infrastructure.Permissions;
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Code { get; set; }
    public PermissionRequirement(string code)
    {
        Code = code;
    }
}
