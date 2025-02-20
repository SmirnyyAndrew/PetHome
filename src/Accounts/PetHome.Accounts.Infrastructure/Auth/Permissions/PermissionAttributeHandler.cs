using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Auth;
using PetHome.Framework.Auth;

namespace PetHome.Accounts.Infrastructure.Auth.Permissions;
public class PermissionAttributeHandler(IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<PermissionAttribute>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        UserScopedData userScopedData = 
            httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<UserScopedData>()!;

        if (userScopedData is null)
            context.Fail();

        if (userScopedData!.Permissions.Contains(requirement.Code))
            context.Succeed(requirement);
         
        return Task.CompletedTask;
    }
}
