using Microsoft.AspNetCore.Authorization;

namespace PetHome.Accounts.Infrastructure.Permissions;
public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        context.Succeed(requirement);

        return Task.CompletedTask;
    } 
}
