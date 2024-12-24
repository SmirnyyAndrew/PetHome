using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Database.Read;
using Xunit;

namespace PetHome.IntegrationTests;
public class BaseTest
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestFactory _factory;
    protected readonly Fixture _fixture; 
    protected readonly IServiceScope _scope;
    protected readonly IReadDBContext _readDbContext;
    protected readonly IReadDBContext _writeDbContext; 
    
    
    public BaseTest(IntegrationTestFactory factory)
    {
        _factory = factory; 
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope(); 
        _readDbContext = _scope.ServiceProvider.GetRequiredService<IReadDBContext>();
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync(); 
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
