using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.IntegrationTests.Features.Write.PetManegment.ChangeSerialNumber;
public class ChangePetSerialNumberUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<string, ChangePetSerialNumberCommand> _sut;

    public ChangePetSerialNumberUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<string, ChangePetSerialNumberCommand>>();
    }

    [Fact]
    public async void Success_changed_pet_serial_number()
    {
        //array
        await SeedVolunteersWithAggregates();
        var pet = _writeDbContext.Volunteers
            .SelectMany(p => p.Pets)
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
