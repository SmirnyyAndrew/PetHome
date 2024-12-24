using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Database.Read;
using Xunit;

namespace PetHome.IntegrationTests;
public class BaseTest
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    private readonly Fixture _fixture; 
    private readonly IServiceScope _scope;
    private readonly IReadDBContext _readDbContext;
    private readonly IReadDBContext _writeDbContext;

    
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
    [Fact]
    public void n()
    {
        Assert.Equal(1, 1);
    }
}
