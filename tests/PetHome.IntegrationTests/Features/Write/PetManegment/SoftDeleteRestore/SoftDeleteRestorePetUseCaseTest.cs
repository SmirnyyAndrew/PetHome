using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Write.PetManegment.SoftDeleteRestore;
using PetHome.Application.Interfaces.FeatureManagment;
using Xunit;

namespace PetHome.IntegrationTests.Features.Write.PetManegment.SoftDeleteRestore;
public class SoftDeleteRestorePetUseCaseTest : BaseTest, IClassFixture<IntegrationTestFactory>
{
    private readonly ICommandHandler<SoftDeleteRestorePetCommand> _sut;
    public SoftDeleteRestorePetUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<SoftDeleteRestorePetCommand>>();
    }

    [Fact]
    public async void Success_soft_delete_restore()
    {
        //array
        await SeedVolunteersWithAggregates();
        var pet = _writeDbContext.Volunteers.SelectMany(p => p.Pets).First();
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
