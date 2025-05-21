using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Features.Write.PetManegment.ChangePetStatus;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetManagementService.IntegrationTests.Features.Write.PetManegment.ChangePetStatus;

[Collection("Pet")]
public class ChangePetStatusUseCaseTest : PetManagementFactory
{
    private readonly ICommandHandler<string, ChangePetStatusCommand> _sut;

    public ChangePetStatusUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<string, ChangePetStatusCommand>>();
    }

    [Fact]
    public async void Change_pet_status()
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
