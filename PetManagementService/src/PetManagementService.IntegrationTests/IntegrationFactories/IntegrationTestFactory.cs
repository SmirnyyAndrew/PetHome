using FilesService.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using PetManagementService.Application.Database;
using PetManagementService.Infrastructure.Database.Read.DBContext;
using PetManagementService.Infrastructure.Database.Write.DBContext;
using PetManagementService.IntegrationTests.Mocks;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Xunit;

namespace PetManagementService.IntegrationTests.IntegrationFactories;

public class IntegrationTestFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_management_service_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner;
    private DbConnection _dbConnection;
    private IMinioFilesHttpClient _fileServiceMock;
    private PetManagementWriteDBContext _volunteerWriteDbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
    }

    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(PetManagementWriteDBContext));
        services.RemoveAll(typeof(IPetManagementReadDbContext));
        services.RemoveAll(typeof(IMinioFilesHttpClient));

        services.AddScoped(_ =>
              new PetManagementWriteDBContext(_dbContainer.GetConnectionString()));
        services.AddScoped<IPetManagementReadDbContext, PetManagementReadDBContext>(_ =>
              new PetManagementReadDBContext(_dbContainer.GetConnectionString()));

        IMinioFilesHttpClient minioClientMock = MinioFilesHttpClientMocker.MockMethods();
        services.AddTransient(_ => minioClientMock);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _volunteerWriteDbContext = Services.CreateScope().ServiceProvider
            .GetRequiredService<PetManagementWriteDBContext>();

        await _volunteerWriteDbContext.Database.EnsureCreatedAsync();

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
            SchemasToInclude = ["public"]
        };
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            respawnerOptions);
    }
}
