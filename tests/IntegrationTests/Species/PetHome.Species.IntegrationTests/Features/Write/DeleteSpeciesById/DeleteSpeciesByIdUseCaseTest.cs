using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Species.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Write.PetManegment.DeleteSpeciesById;
using Xunit;

namespace PetHome.Species.IntegrationTests.Features.Write.DeleteSpeciesById;
public class DeleteSpeciesByIdUseCaseTest : SpeciesFactory
{
    private readonly ICommandHandler<string, DeleteSpeciesByIdCommand> _sut;
    public DeleteSpeciesByIdUseCaseTest(IntegrationTestFactory factory) : base(factory)
    { 
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<string, DeleteSpeciesByIdCommand>>();
    }

    [Fact]
    public async void Success_delete_species_by_id()
    {
        //array
       await SeedSpecies(1);
        var species = _readDbContext.Species.First();
        DeleteSpeciesByIdCommand command = new DeleteSpeciesByIdCommand(species.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
