using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.ChangeSerialNumber;
using PetHome.Volunteers.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Volunteers.IntegrationTests.Features.Write.PetManegment.ChangeSerialNumber;
public class ChangePetSerialNumberUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<string, ChangePetSerialNumberCommand> _sut;

    public ChangePetSerialNumberUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<string, ChangePetSerialNumberCommand>>();
    }

    [Fact]
    public async void Changed_pet_serial_number()
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
