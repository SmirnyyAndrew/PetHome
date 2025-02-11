using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using NSubstitute;
using PetHome.Core.Interfaces;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.File;
using PetHome.Species.Application.Database;
using PetHome.Species.Infrastructure.Database.Read.DBContext;
using PetHome.Species.Infrastructure.Database.Write.DBContext;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Infrastructure.Database.Read.DBContext;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Xunit;

namespace PetHome.Volunteers.IntegrationTests.IntegrationFactories;

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
    private IFilesProvider _fileServiceMock = Substitute.For<IFilesProvider>();
    private VolunteerWriteDbContext _volunteerWriteDbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(ConfigureDefault);
    }

    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(ISpeciesReadDbContext)); 
        services.RemoveAll(typeof(IVolunteerReadDbContext));  
        services.RemoveAll(typeof(SpeciesWriteDbContext));
        services.RemoveAll(typeof(VolunteerWriteDbContext)); 
        services.RemoveAll(typeof(IFilesProvider));

          
        services.AddScoped(_ =>
               new SpeciesWriteDbContext(_dbContainer.GetConnectionString())); 
        services.AddScoped(_ =>
              new VolunteerWriteDbContext(_dbContainer.GetConnectionString())); 
        services.AddScoped<IVolunteerReadDbContext, VolunteerReadDbContext>(_ =>
              new VolunteerReadDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
              new SpeciesReadDbContext(_dbContainer.GetConnectionString()));

        services.AddTransient(_ => _fileServiceMock);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _volunteerWriteDbContext = Services.CreateScope().ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();

        await _volunteerWriteDbContext.Database.EnsureCreatedAsync();

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
            SchemasToInclude = ["public"]
        };
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            respawnerOptions);
    }

    public void SetupSuccessFileServiceMock()
    {
        var response = MediaFile.Create("photos", "test_file_name").Value;

        _fileServiceMock.UploadFile(
               Arg.Any<Stream>(),
               Arg.Any<MinioFileInfoDto>(),
               false,
               CancellationToken.None)
            .Returns(Result.Success<MediaFile, Error>(response));
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
