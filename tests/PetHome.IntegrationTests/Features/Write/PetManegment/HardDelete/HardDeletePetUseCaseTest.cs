using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Write.PetManegment.HardDelete;
using Xunit;
namespace PetHome.IntegrationTests.Features.Write.PetManegment.HardDelete;
public class HardDeletePetUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<HardDeletePetCommand> _sut;
    public HardDeletePetUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<HardDeletePetCommand>>();
    }

    [Fact]
    public async void Success_delete_species_by_id()
    {
        //array
        await SeedVolunteersWithAggregates();
        var pet = _volunteerWriteDbContext.Volunteers.SelectMany(p => p.Pets).First();
        HardDeletePetCommand command = new HardDeletePetCommand(pet.VolunteerId, pet.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}