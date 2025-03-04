using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database;
using PetHome.Accounts.Application.Features.Contracts.AuthManagement.GetRole;
using PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateAccessToken;
using PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateRefreshToken;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetPermissionsNames;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetRolesNames;
using PetHome.Accounts.Application.Features.Write.CreateParticipant;
using PetHome.Accounts.Application.Features.Write.CreateUser;
using PetHome.Accounts.Application.Features.Write.CreateVolunteer;
using PetHome.Accounts.Contracts.Contracts.AuthManagement;
using PetHome.Accounts.Contracts.Contracts.TokensManagment.AccessToken.GenerateAccessToken;
using PetHome.Accounts.Contracts.Contracts.TokensManagment.RefreshToken.GenerateRefreshToken;
using PetHome.Accounts.Contracts.Contracts.UserManagment;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Framework.Auth;
using System.Net.NetworkInformation;

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
         
        services.AddScoped<IGetRoleContract, GetRoleUsingContract>();
        services.AddScoped<IGetUserRoleNameContract, GetUserRoleNameUsingContract>();   

        services.AddScoped<IGetUserPermissionsCodesContact, GetUserPermissionsCodesUsingContact>();
        services.AddScoped<IGenerateAccessTokenContract, GenerateAccessTokenUsingContract>();
        services.AddScoped<IGenerateRefreshTokenContract, GenerateRefreshTokenUsingContract>();

        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);

        services.AddHttpContextAccessor()
            .AddScoped<UserScopedData>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
        });

        return services;
    }
}
