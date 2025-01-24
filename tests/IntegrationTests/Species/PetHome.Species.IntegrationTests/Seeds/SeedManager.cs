using Microsoft.Extensions.DependencyInjection;
using PetHome.Species.Infrastructure.Database.Write.DBContext;
using PetHome.Species.IntegrationTests.IntegrationFactories;

namespace PetHome.Species.IntegrationTests.Seeds.Species;
public partial class SeedManager
{
    protected readonly SpeciesWriteDbContext _writeDbContext; 
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
    }
}
