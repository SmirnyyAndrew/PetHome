using DiscussionService.Infrastructure.Database.Write;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Discussions.IntegrationTests.IntegrationFactories;

namespace PetHome.Discussions.IntegrationTests.Seeds;
public partial class SeedManager
{
    protected readonly DiscussionDbContext _dbContext; 
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<DiscussionDbContext>();
    }
}
