using Microsoft.AspNetCore.Authorization;
using PetHome.Framework.Auth.Database;
using PetHome.SharedKernel.ValueObjects.AuthAggregates.RolePermission;
using PetHome.SharedKernel.ValueObjects.AuthAggregates.User;
using System.IdentityModel.Tokens.Jwt;

namespace PetHome.Accounts.Infrastructure.Auth.Permissions;
public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly AuthorizationDbContext _dbContext;

    public PermissionRequirementHandler(AuthorizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        string? userId = context.User.Claims.Select(x => JwtRegisteredClaimNames.Sub).FirstOrDefault();
        User? user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user is null)
            return Task.FromResult<AuthorizationPolicy>(null!);

        RoleId? userRoleId = user.RoleId;
        Role? userRole = _dbContext.Roles.FirstOrDefault(u => u.Id == userRoleId);

        bool? hasPermission = userRole?.Permissions.Any(p => p.Code.Value == requirement.Code);

        if (hasPermission is not true) 
            return Task.FromResult<AuthorizationPolicy>(null!); 
         
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
