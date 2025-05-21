using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;
namespace PetManagementService.IntegrationTests.Features.Write.VolunteerManegment.HardDeleteVolunteer;

[Collection("Volunteer")]
public class HardDeleteVolunteerUseCaseTest : PetManagementFactory
{
    private readonly ICommandHandler<bool, HardDeleteVolunteerCommand> _sut;

    public HardDeleteVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<bool, HardDeleteVolunteerCommand>>();
    }

    [Fact]
    public async void Hard_delete_volunteer()
    { 
        //array 
        await SeedVolunteers(1);
        var volunteer = _writeDbContext.Volunteers.First();
        HardDeleteVolunteerCommand command = new HardDeleteVolunteerCommand(volunteer.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
