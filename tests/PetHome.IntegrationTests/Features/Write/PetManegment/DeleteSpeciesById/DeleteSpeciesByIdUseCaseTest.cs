using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Write.PetManegment.DeleteSpeciesById;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.IntegrationTests.Features.Write.PetManegment.DeleteSpeciesById;
public class DeleteSpeciesByIdUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<string, DeleteSpeciesByIdCommand> _sut;
    public DeleteSpeciesByIdUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<string, DeleteSpeciesByIdCommand>>();
    }

    [Fact]
    public async void Success_delete_species_by_id()
    {
        //array
        await SeedSpecies(1);
        var species = _writeDbContext.Species.First();
        DeleteSpeciesByIdCommand command = new DeleteSpeciesByIdCommand(species.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
