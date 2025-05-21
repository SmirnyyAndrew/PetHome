using Microsoft.Extensions.DependencyInjection;
using PetManagementService.Infrastructure.Database.Write.DBContext;
using PetManagementService.IntegrationTests.IntegrationFactories;

namespace PetManagementService.IntegrationTests.Seeds.SpeciesEntity;
public partial class SeedManager
{
    protected readonly PetManagementWriteDBContext _writeDbContext;
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope.ServiceProvider.GetRequiredService<PetManagementWriteDBContext>();
    }
}
