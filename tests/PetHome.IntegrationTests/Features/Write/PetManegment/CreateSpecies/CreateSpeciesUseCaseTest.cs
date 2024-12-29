using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Species.Application.Features.Write.CreateSpecies;
using Xunit;
namespace PetHome.IntegrationTests.Features.Write.PetManegment.CreateSpecies;
public class CreateSpeciesUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;
    public CreateSpeciesUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
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
