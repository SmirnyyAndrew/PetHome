using AccountService.Application.Database.Repositories;
using AccountService.Infrastructure.Database;
using AccountService.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Tests.IntegrationTests.Constants;
using PetHome.Core.Tests.IntegrationTests.DependencyInjections;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Xunit;

namespace AccountService.IntegrationTests.IntegrationFactories;

public class IntegrationTestFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("account_service_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner;
    private DbConnection _dbConnection;
    private AuthorizationDbContext _dbContext;
    private IAuthenticationRepository _repository;
    private IUnitOfWork _unitOfWork;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
        builder.AddEnvironment(Environments.Test);
    }


    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(AuthorizationDbContext));
        services.RemoveAll(typeof(IAuthenticationRepository));


        _repository = new AuthenticationRepository(new AuthorizationDbContext(_dbContainer.GetConnectionString()), default);
        _unitOfWork = new UnitOfWork(new AuthorizationDbContext(_dbContainer.GetConnectionString()));

        services.AddScoped(_ => new AuthorizationDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped(_ => _repository);
        services.AddScoped(_ => _unitOfWork);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<AuthorizationDbContext>();

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
            SchemasToInclude = ["Account"]
        };
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            respawnerOptions);
    }
}
