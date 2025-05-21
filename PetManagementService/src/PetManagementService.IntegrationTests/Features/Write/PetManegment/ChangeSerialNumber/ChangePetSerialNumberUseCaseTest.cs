using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Dto.Pet;
using PetManagementService.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetManagementService.IntegrationTests.Features.Write.PetManegment.ChangeSerialNumber;

[Collection("Pet")]
public class ChangePetSerialNumberUseCaseTest : PetManagementFactory
{
    private readonly ICommandHandler<string, ChangePetSerialNumberCommand> _sut;

    public ChangePetSerialNumberUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService
            <ICommandHandler<string, ChangePetSerialNumberCommand>>();
    }
    [Fact]
    public async void Change_pet_serial_number()
    {
        //array 
        await SeedVolunteersWithAggregates();
        var pet = _writeDbContext.Volunteers
            .SelectMany(p => p.Pets)
            .ToList()
            .First();

        ChangePetSerialNumberDto dto = new ChangePetSerialNumberDto(pet.Id, 1);
        ChangePetSerialNumberCommand command = new ChangePetSerialNumberCommand(
            pet.VolunteerId,
            dto);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
