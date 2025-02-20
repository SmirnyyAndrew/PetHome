using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database;
using PetHome.Accounts.Application.Features.Contracts.TokensManagment.AccessToken.GenerateAccessToken;
using PetHome.Accounts.Application.Features.Contracts.TokensManagment.RefreshToken.GenerateRefreshToken;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.CreateRole;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.CreateUser;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetPermissions;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetRoles;
using PetHome.Accounts.Contracts.TokensManagment.AccessToken.GenerateAccessToken;
using PetHome.Accounts.Contracts.TokensManagment.RefreshToken.GenerateRefreshToken;
using PetHome.Accounts.Contracts.UserManagment;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Framework.Auth;

namespace PetHome.Accounts.Application;
public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddAccountsServices(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(ApplicationDependencyInjection).Assembly)
        .AddClasses(classes => classes
            .AssignableToAny(
                typeof(ICommandHandler<>), typeof(ICommandHandler<,>),
                typeof(IQueryHandler<>), typeof(IQueryHandler<,>)))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());

        services.AddScoped<ICreateUserContract, CreateUserUsingContract>();
        services.AddScoped<IGetRoleContract, GetRoleUsingContract>();
        services.AddScoped<IGetUserRoleNameContract, GetUserRoleNameUsingContract>();
        services.AddScoped<IGetUserPermissionsCodesContact, GetUserPermissionsCodesUsingContact>();
        services.AddScoped<IGenerateAccessTokenContract, GenerateAccessTokenUsingContract>();
        services.AddScoped<IGenerateRefreshTokenContract, GenerateRefreshTokenUsingContract>();
          
        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);

        services.AddHttpContextAccessor()
            .AddScoped<UserScopedData>();

        return services;
    }
}
