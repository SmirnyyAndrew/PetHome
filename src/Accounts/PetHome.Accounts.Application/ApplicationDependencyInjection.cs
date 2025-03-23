using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Features.Contracts.AuthManagement.GetRole;
using PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateAccessToken;
using PetHome.Accounts.Application.Features.Contracts.TokenManagment.GenerateRefreshToken;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetPermissionsNames;
using PetHome.Accounts.Application.Features.Contracts.UserManagment.GetRolesNames;
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
