using Microsoft.Extensions.DependencyInjection;
using PetHome.VolunteerRequests.Infrastructure.Database;
using PetHome.VolunteerRequests.IntegrationTests.IntegrationFactories;

namespace PetHome.VolunteerRequests.IntegrationTests.Seeds;
public partial class SeedManager
{
    protected readonly VolunteerRequestDbContext _writeDbContext; 
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope.ServiceProvider.GetRequiredService<VolunteerRequestDbContext>();
    }
}
