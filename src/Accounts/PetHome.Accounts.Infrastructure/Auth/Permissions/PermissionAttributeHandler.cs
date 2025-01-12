using Microsoft.AspNetCore.Authorization;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Accounts.Infrastructure.Database;
using PetHome.Core.Auth;
using PetHome.Core.ValueObjects.RolePermission;

namespace PetHome.Accounts.Infrastructure.Auth.Permissions;
public class PermissionAttributeHandler : AuthorizationHandler<PermissionAttribute>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        AuthorizationDbContext _dbContext = new AuthorizationDbContext();

        string? userId = context.User.Claims.FirstOrDefault()?.Value; 
         
        User? user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user is null)
        {
            context.Fail();
            return Task.FromResult<AuthorizationPolicy>(null!);
        }

        RoleId? userRoleId = user.RoleId;
        Permission? permission = _dbContext.Permissions.ToList()
            .FirstOrDefault(p => p.Code.Value == requirement.Code);
        if(permission is null)
        {
            context.Fail();
            return Task.FromResult<AuthorizationPolicy>(null!);
        }

        bool? hasPermission = _dbContext.RolesPermissions
            .Any(p => p.PermissionId == permission.Id && p.RoleId == userRoleId);

        if (hasPermission is not true)
        {
            context.Fail();
            return Task.FromResult<AuthorizationPolicy>(null!);
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
