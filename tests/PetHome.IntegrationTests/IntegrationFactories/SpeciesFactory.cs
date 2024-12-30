using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHome.IntegrationTests.Seeds;
using PetHome.Species.Application.Database;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Infrastructure.Database.Write.DBContext;
using Xunit;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class SpeciesFactory : BaseFactory
{
    private readonly SeedManager _seedManager;
    protected readonly ISpeciesReadDbContext _readDbContext;
    protected readonly SpeciesWriteDbContext _writeDbContext;
    public SpeciesFactory(IntegrationTestFactory factory) : base(factory)
    {
        _readDbContext = _scope.ServiceProvider.GetRequiredService<ISpeciesReadDbContext>();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        _seedManager = new SeedManager(factory);
    }

    protected IReadOnlyList<_Species> SeedSpecies(int speciesCountToSeed = 3)
        => _seedManager.SeedSpecies(5).Result;

    protected IReadOnlyList<Breed> SeedBreeds(int breedCountToSedd = 5)
        => _seedManager.SeedBreeds(breedCountToSedd).Result;

    
}
