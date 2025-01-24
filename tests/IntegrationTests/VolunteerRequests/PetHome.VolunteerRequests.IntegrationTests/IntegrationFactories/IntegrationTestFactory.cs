using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using PetHome.Accounts.Application.Features.Contracts.CreateRole;
using PetHome.Accounts.Application.Features.Contracts.CreateUser;
using PetHome.Accounts.Contracts;
using PetHome.VolunteerRequests.Infrastructure.Database.Write;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Xunit;

namespace PetHome.VolunteerRequests.IntegrationTests.IntegrationFactories;

public class IntegrationTestFactory
    : WebApplicationFactory<PetHome.API.Program>, IAsyncLifetime
{
    protected readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_home_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner;
    private DbConnection _dbConnection;
    private VolunteerRequestDbContext _writeDbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(ConfigureDefault);
    }

    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(VolunteerRequestDbContext));
        services.RemoveAll(typeof(IGetRoleContract));
        services.RemoveAll(typeof(ICreateUserContract));

        services.AddScoped(_ =>
              new VolunteerRequestDbContext(_dbContainer.GetConnectionString()));

        services.AddScoped<ICreateUserContract, CreateUserUsingContract>();
        services.AddScoped<IGetRoleContract, GetRoleUsingContract>();

    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _writeDbContext = Services.CreateScope().ServiceProvider.GetRequiredService<VolunteerRequestDbContext>();

        await _writeDbContext.Database.EnsureCreatedAsync();

        await InilizeRespawner();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await ResetDatabaseAsync();
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
