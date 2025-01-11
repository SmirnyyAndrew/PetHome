using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Species.Application.Features.Write.CreateSpecies;
using Xunit;
namespace PetHome.IntegrationTests.Features.Species.Write.CreateSpecies;
public class CreateSpeciesUseCaseTest : SpeciesFactory
{
    private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;
    public CreateSpeciesUseCaseTest(IntegrationTestFactory factory) : base(factory)
    { 
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
    }

    [Fact]
    public async void Success_create_species()
    {
        //array
        CreateSpeciesCommand command = new CreateSpeciesCommand("Собака");

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
