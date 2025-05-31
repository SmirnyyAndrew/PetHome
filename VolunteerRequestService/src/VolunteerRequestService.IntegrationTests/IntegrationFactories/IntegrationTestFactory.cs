using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using PetHome.Core.Tests.Constants;
using PetHome.Core.Tests.IntegrationTests.DependencyInjections;
using PetHome.VolunteerRequests.Infrastructure.Database.Write;
using PetHome.VolunteerRequests.Infrastructure.Database.Write.Repositories;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Xunit;

namespace VolunteerRequestService.IntegrationTests.IntegrationFactories;

public class IntegrationTestFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("volunteer_request_service_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner;
    private DbConnection _dbConnection;
    private VolunteerRequestDbContext _dbContext;
    private VolunteerRequestRepository _repository;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
        builder.AddEnvironment(Environments.Test);
    }

    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(VolunteerRequestDbContext));

        _repository = new VolunteerRequestRepository(new VolunteerRequestDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped(_ => _repository);
        services.AddScoped(_ => new VolunteerRequestDbContext(_dbContainer.GetConnectionString()));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<VolunteerRequestDbContext>();

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
            SchemasToInclude = ["VolunteerRequests"]
        };
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            respawnerOptions);
    }
}
