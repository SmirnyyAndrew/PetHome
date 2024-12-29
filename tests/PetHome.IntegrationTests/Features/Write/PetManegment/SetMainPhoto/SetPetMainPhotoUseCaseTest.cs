using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Write.PetManegment.SetMainPhoto;
using Xunit;
namespace PetHome.IntegrationTests.Features.Write.PetManegment.SetMainPhoto;
public class SetPetMainPhotoUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<SetPetMainPhotoCommand> _sut;
    public SetPetMainPhotoUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<SetPetMainPhotoCommand>>();
    }

    [Fact]
    public async void Success_set_pet_main_info()
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
