using Microsoft.Extensions.DependencyInjection;
using PetHome.VolunteerRequests.Infrastructure.Database.Write;
using VolunteerRequestService.IntegrationTests.IntegrationFactories;

namespace VolunteerRequestService.IntegrationTests.Seeds;
public partial class SeedManager
{
    protected readonly VolunteerRequestDbContext _writeDbContext;
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope.ServiceProvider.GetRequiredService<VolunteerRequestDbContext>();
    }
}
