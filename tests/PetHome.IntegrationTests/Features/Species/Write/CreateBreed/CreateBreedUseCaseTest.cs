using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Species.Application.Features.Write.CreateBreed;
using Xunit;
namespace PetHome.IntegrationTests.Features.Species.Write.CreateBreed;
public class CreateBreedUseCaseTest : SpeciesFactory
{
    private readonly ICommandHandler<Guid, CreateBreedCommand> _sut;
    public CreateBreedUseCaseTest(IntegrationTestFactory factory) : base(factory)
    { 
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateBreedCommand>>();
    }

    [Fact]
    public async void Success_create_breed()
    {
        //array
        SeedSpecies(1);
        var species = _writeDbContext.Species.First();
        var breedNames = new List<string>() { "Алабай", "Овчарка", "Питбуль" };

        CreateBreedCommand command = new CreateBreedCommand(species.Id, breedNames);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
