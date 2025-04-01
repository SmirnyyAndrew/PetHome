using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PetHome.Core.Auth;

namespace AccountService.Infrastructure.Auth.Permissions;

/// <summary>
/// Provider для проверки, есть ли у пользователя необходимые разрешения
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        string formattedPolicyName = policyName.Trim().ToLower();

        if (string.IsNullOrEmpty(formattedPolicyName))
            return Task.FromResult<AuthorizationPolicy?>(null);


        var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
          .RequireAuthenticatedUser()
          .AddRequirements(new PermissionAttribute(formattedPolicyName))
          .Build();

        return Task.FromResult<AuthorizationPolicy>(policy);
    }
}
