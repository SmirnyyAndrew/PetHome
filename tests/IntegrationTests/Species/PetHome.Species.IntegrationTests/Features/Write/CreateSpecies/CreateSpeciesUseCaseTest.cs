using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Species.Application.Features.Write.CreateSpecies;
using PetHome.Species.IntegrationTests.IntegrationFactories;
using Xunit;
namespace PetHome.Species.IntegrationTests.Features.Write.CreateSpecies;
public class CreateSpeciesUseCaseTest : SpeciesFactory
{
    private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;
    public CreateSpeciesUseCaseTest(IntegrationTestFactory factory) : base(factory)
    { 
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
    }

    [Fact]
    public async void Create_species()
    {
        //array
        CreateSpeciesCommand command = new CreateSpeciesCommand("Собака");

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
