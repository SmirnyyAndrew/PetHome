using AccountService.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.SharedKernel.Options.Background;

namespace AccountService.Infrastructure.BackgroundServices;
internal class SoftDeletableEntitiesMonitor : BackgroundService
{
    private readonly SoftDeletableEntitiesOption _option;
    private AuthorizationDbContext _dbContext;
    private readonly IServiceScopeFactory _scopeFactory;

    public SoftDeletableEntitiesMonitor(
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _option = configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME)
            .Get<SoftDeletableEntitiesOption>();

        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(_option.HoursToCheck)))
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await timer.WaitForNextTickAsync();
                HardDeleteExpiredSoftDeletedEntities();
            }
        }
    }

    private async void HardDeleteExpiredSoftDeletedEntities(CancellationToken ct = default)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var hardDeleteSoftDeletedEntitiesServices = scope.ServiceProvider.GetServices<IHardDeleteSoftDeletedEntitiesContract>();
            var hardDeleteTasks = hardDeleteSoftDeletedEntitiesServices.Select(task => task.HardDeleteExpiredSoftDeletedEntities(ct));

            await Task.WhenAll(hardDeleteTasks); 
        }
    }
}
