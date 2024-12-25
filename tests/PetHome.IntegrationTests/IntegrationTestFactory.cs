using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using NSubstitute;
using PetHome.Application.Database.Read;
using PetHome.Application.Interfaces;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.Shared.Error;
using PetHome.Infrastructure.DataBase.Read.DBContext;
using PetHome.Infrastructure.DataBase.Write.DBContext;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Xunit;

namespace PetHome.IntegrationTests;

public class IntegrationTestFactory
    : WebApplicationFactory<API.Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_home_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner;
    private DbConnection _dbConnection;
    private IFilesProvider _fileServiceMock = Substitute.For<IFilesProvider>();
    private WriteDBContext _writeDbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
    }

    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(IReadDBContext));
        services.RemoveAll(typeof(WriteDBContext));
        services.RemoveAll(typeof(IFilesProvider));
         
        services.AddScoped<WriteDBContext>(_ =>
               new WriteDBContext(_dbContainer.GetConnectionString()));
        services.AddScoped<IReadDBContext, ReadDBContext>(_ =>
              new ReadDBContext(_dbContainer.GetConnectionString())); 
        services.AddTransient<IFilesProvider>(_ => _fileServiceMock);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _writeDbContext = Services.CreateScope().ServiceProvider.GetRequiredService<WriteDBContext>();

        await _writeDbContext.Database.EnsureDeletedAsync();
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
        await _respawner.ResetAsync(_dbConnection);
    }

    private async Task InilizeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions()
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }

    public void SetupSuccessFileServiceMock()
    {
        var response = Media.Create("photos", "test_file_name").Value;

        _fileServiceMock.UploadFile(
               Arg.Any<Stream>(),
               Arg.Any<MinioFileInfoDto>(),
               false,
               CancellationToken.None)
            .Returns(Result.Success<Media, Error>(response));
    }

    public void SetupFailedFileServiceMock()
    {
        _fileServiceMock.UploadFile(
                Arg.Any<Stream>(),
                Arg.Any<MinioFileInfoDto>(),
                false,
                Arg.Any<CancellationToken>())
            .Returns(Errors.Failure("Интеграционный тест"));
    }
}
