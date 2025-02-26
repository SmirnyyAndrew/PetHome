using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Contracts.UserManagment;
using PetHome.Framework.Auth;
using System.IdentityModel.Tokens.Jwt;

namespace PetHome.SharedKernel.Middlewares;
public class UserScopedDataMiddleware(
    IServiceScopeFactory factory, RequestDelegate next)
{

    public async Task InvokeAsync(
        HttpContext httpContext, UserScopedData userScopedData)
    {
        if (httpContext.User.Identity is null || httpContext.User.Identity.IsAuthenticated is false)
        {
            await next(httpContext);
            return;
        }

        string userIdClaim = httpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
        string userEmail = httpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value;

        if (Guid.TryParse(userIdClaim, out Guid userId) == false)
        {
            await next(httpContext);
            return;
        }

        if (userScopedData.UserId == userId)
        {
            await next(httpContext);
            return;
        }

        var getRolesNamesContract = factory.CreateScope().ServiceProvider.GetService<IGetUserRoleNameContract>();
        var getPermissionsNamesContract = factory.CreateScope().ServiceProvider.GetService<IGetUserPermissionsCodesContact>();

        userScopedData.UserId = userId;
        userScopedData.Email = userEmail;
        userScopedData.Role = getRolesNamesContract.Execute(userId, CancellationToken.None).Result;
        userScopedData.Permissions = getPermissionsNamesContract.Execute(userId, CancellationToken.None).Result;
    }
}
