using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Application.Database;
using PetHome.Accounts.Application.Features.Contracts.CreateRole;
using PetHome.Accounts.Application.Features.Contracts.CreateUser;
using PetHome.Accounts.Contracts;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;

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

        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
      
        return services;
    }
}
