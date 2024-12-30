using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Write.PetManegment.SoftDeleteRestore;
using Xunit;
namespace PetHome.IntegrationTests.Features.Volunteer.Write.PetManegment.SoftDeleteRestorePet;
public class SoftDeleteRestorePetUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<SoftDeleteRestorePetCommand> _sut;
    public SoftDeleteRestorePetUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<SoftDeleteRestorePetCommand>>();
    }

    [Fact]
    public async void Success_soft_delete_restore_pet()
    {
        //array
        await SeedVolunteersWithAggregates();
        var pet = _volunteerWriteDbContext.Volunteers.SelectMany(p => p.Pets).First();
        SoftDeleteRestorePetCommand command = new SoftDeleteRestorePetCommand(
            pet.VolunteerId,
            pet.Id,
            true);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
