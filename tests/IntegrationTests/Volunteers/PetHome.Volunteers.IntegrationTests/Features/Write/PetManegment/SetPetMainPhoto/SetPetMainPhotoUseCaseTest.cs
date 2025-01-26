using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;
using PetHome.Volunteers.IntegrationTests.IntegrationFactories;
using Xunit;
namespace PetHome.Volunteers.IntegrationTests.Features.Write.PetManegment.SetPetMainPhoto;
public class SetPetMainPhotoUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<SetPetMainPhotoCommand> _sut;
    public SetPetMainPhotoUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SetPetMainPhotoCommand>>();
    }

    [Fact]
    public async void Set_pet_main_info()
    {
        //array
        await SeedVolunteersWithAggregates();
        var pet = _writeDbContext.Volunteers.SelectMany(p => p.Pets).First();
        SetPetMainPhotoCommand command = new SetPetMainPhotoCommand(
            pet.VolunteerId,
            pet.Id,
            "photos",
            Guid.NewGuid().ToString());

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
