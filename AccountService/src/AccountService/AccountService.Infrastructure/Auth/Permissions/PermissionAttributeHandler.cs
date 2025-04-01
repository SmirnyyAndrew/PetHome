using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Auth;
using PetHome.Framework.Auth;

namespace AccountService.Infrastructure.Auth.Permissions;

/// <summary>
/// Обработчик для получения разрешений пользователя
/// </summary>
/// <param name="httpContextAccessor">Получить доступ к текущему http-запросу и его сервисам</param>
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
        {
            context.Fail();
            return Task.FromResult<AuthorizationPolicy>(null!);
        }

        if (userScopedData!.Permissions.Contains(requirement.Code))
        {
            context.Succeed(requirement); 
            return Task.FromResult<AuthorizationPolicy>(null!);
        }
         
        return Task.CompletedTask;
    }
}
