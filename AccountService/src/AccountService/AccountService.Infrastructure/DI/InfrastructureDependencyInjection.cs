using AccountService.Application.Database.Repositories;
using AccountService.Infrastructure.Auth.Permissions;
using AccountService.Infrastructure.Contracts;
using AccountService.Infrastructure.Database;
using AccountService.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Framework.Database;
using PetHome.SharedKernel.Options.Accounts;
using PetHome.SharedKernel.Options.Background;
using AccountService.Infrastructure.DI.TransactionOutbox;
using AccountService.Infrastructure.TransactionOutbox;

namespace AccountService.Infrastructure.DI;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthorizationDbContext>(_ =>
            new AuthorizationDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.Configure<AdminOption>(configuration.GetSection(AdminOption.SECTION_NAME));
        services.Configure<RefreshTokenOption>(configuration.GetSection(RefreshTokenOption.SECTION_NAME));
        services.Configure<JwtOption>(configuration.GetSection(JwtOption.SECTION_NAME));
        services.Configure<SoftDeletableEntitiesOption>(configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME));

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.ACCOUNT_UNIT_OF_WORK_KEY);
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddScoped<IRefreshSessionRepository, RefreshSessionRepository>();
        services.AddSingleton<IAuthorizationHandler, PermissionAttributeHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        //services.AddHostedService<SoftDeletableEntitiesMonitor>();

        services.AddScoped<IHardDeleteSoftDeletedEntitiesContract, HardDeleteExpiredSoftDeletedAccountEntitiesContract>(); 

        services.AddOutboxService();

        return services;
    }
}
