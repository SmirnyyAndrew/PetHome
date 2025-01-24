using Microsoft.Extensions.DependencyInjection;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using PetHome.Volunteers.IntegrationTests.IntegrationFactories;

namespace PetHome.Volunteers.IntegrationTests.Seeds.Species;
public partial class SeedManager
{
    protected readonly VolunteerWriteDbContext _writeDbContext; 
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
    }
}
