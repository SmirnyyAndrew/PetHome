using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Features.Write.PetManegment.SoftDeleteRestore;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;
namespace PetManagementService.IntegrationTests.Features.Write.PetManegment.SoftDeleteRestorePet;

[Collection("Pet")]
public class SoftDeleteRestorePetUseCaseTest : PetManagementFactory
{
    private readonly ICommandHandler<SoftDeleteRestorePetCommand> _sut;
    private readonly IntegrationTestFactory factory;

    //private readonly PetManagementWriteDBContext _writeDbContext;

    public SoftDeleteRestorePetUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SoftDeleteRestorePetCommand>>();
        this.factory = factory;
        //_writeDbContext = WriteDbContextHelper.CreateDbContext(factory);
    }

    [Fact]
    public async void Soft_delete_restore_pet()
    {

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
}
