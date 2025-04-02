using AccountService.Application.Database.Repositories;
using AccountService.Infrastructure.Auth.Permissions;
using AccountService.Infrastructure.Contracts;
using AccountService.Infrastructure.Database;
using AccountService.Infrastructure.Database.Repositories;
using AccountService.Infrastructure.DI.TransactionOutbox;
using AccountService.Infrastructure.TransactionOutbox;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Options.Accounts;
using PetHome.Core.Web.Options.Background;
using PetHome.SharedKernel.Constants;

namespace AccountService.Infrastructure.DI;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthorizationDbContext>(_ =>
            new AuthorizationDbContext(configuration.GetConnectionString(Constants.Database.DATABASE)!));

        services.Configure<AdminOption>(configuration.GetSection(AdminOption.SECTION_NAME));
        services.Configure<RefreshTokenOption>(configuration.GetSection(RefreshTokenOption.SECTION_NAME));
        services.Configure<JwtOption>(configuration.GetSection(JwtOption.SECTION_NAME));
        services.Configure<SoftDeletableEntitiesOption>(configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME));

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constants.Database.ACCOUNT_UNIT_OF_WORK_KEY);
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
