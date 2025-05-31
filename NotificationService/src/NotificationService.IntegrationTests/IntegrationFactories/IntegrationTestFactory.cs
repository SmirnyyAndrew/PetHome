using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; 
using NotificationService.Infrastructure.Database;
using Npgsql;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Tests.Constants;
using PetHome.Core.Tests.IntegrationTests.DependencyInjections;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Xunit;

namespace NotificationService.IntegrationTests.IntegrationFactories;
public class IntegrationTestFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("notification_service_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner;
    private DbConnection _dbConnection;
    private NotificationDbContext _dbContext; 
    private NotificationRepository _repository;
    private IUnitOfWork _unitOfWork;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
        builder.AddEnvironment(Environments.Test);
    }


    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(NotificationDbContext));
        services.RemoveAll(typeof(NotificationRepository)); 

        _dbContext = new NotificationDbContext(_dbContainer.GetConnectionString());
        _repository = new NotificationRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext); 

        services.AddScoped(_ => _dbContext);
        services.AddScoped(_ => _repository);
        services.AddScoped(_ => _unitOfWork); 
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<NotificationDbContext>(); 

        await _dbContext.Database.EnsureCreatedAsync();
        await InilizeRespawner();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await ResetDatabaseAsync();
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        if (_respawner is not null)
            await _respawner.ResetAsync(_dbConnection);
    }

    private async Task InilizeRespawner()
    {
        await _dbConnection.OpenAsync();

        RespawnerOptions respawnerOptions = new RespawnerOptions()
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["Notifications"]
        };
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            respawnerOptions);
    }
}
