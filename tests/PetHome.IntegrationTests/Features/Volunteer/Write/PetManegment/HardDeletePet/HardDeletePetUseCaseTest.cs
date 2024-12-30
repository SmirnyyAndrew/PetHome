using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Write.PetManegment.HardDelete;
using Xunit;
namespace PetHome.IntegrationTests.Features.Volunteer.Write.PetManegment.HardDeletePet;
public class HardDeletePetUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<HardDeletePetCommand> _sut;
    public HardDeletePetUseCaseTest(IntegrationTestFactory factory) : base(factory)
    { 
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<HardDeletePetCommand>>();
    }

    [Fact]
    public async void Success_hard_delete_pet_by_id()
    {
        //array
        SeedVolunteersWithAggregates();
        var pet = _writeDbContext.Volunteers.SelectMany(p => p.Pets).First();
        HardDeletePetCommand command = new HardDeletePetCommand(pet.VolunteerId, pet.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}