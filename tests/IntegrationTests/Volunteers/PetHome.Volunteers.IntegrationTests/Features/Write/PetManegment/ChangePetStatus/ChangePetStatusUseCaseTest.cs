using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetStatus;
using PetHome.Volunteers.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Volunteers.IntegrationTests.Features.Write.PetManegment.ChangePetStatus;
public class ChangePetStatusUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<string, ChangePetStatusCommand> _sut;

    public ChangePetStatusUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<string, ChangePetStatusCommand>>();
    }

    [Fact]
    public async void Success_change_pet_status()
    {
        //array
        await SeedVolunteersWithAggregates();
        var pet = _writeDbContext.Volunteers.SelectMany(p => p.Pets).First();

        ChangePetStatusCommand command = new ChangePetStatusCommand(
            pet.VolunteerId,
            pet.Id,
            PetStatusEnum.isFree);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
