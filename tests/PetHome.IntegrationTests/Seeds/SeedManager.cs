using Microsoft.Extensions.DependencyInjection;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

namespace PetHome.IntegrationTests.Seeds;
public partial class SeedManager
{
    protected readonly VolunteerWriteDbContext _writeDbContext; 
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
    }
}
