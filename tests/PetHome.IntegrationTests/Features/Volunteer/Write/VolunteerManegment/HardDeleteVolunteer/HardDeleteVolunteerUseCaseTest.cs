using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using Xunit;
namespace PetHome.IntegrationTests.Features.Volunteer.Write.VolunteerManegment.HardDeleteVolunteer;
public class HardDeleteVolunteerUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<bool, HardDeleteVolunteerCommand> _sut;
    public HardDeleteVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    { 
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<bool, HardDeleteVolunteerCommand>>();
    }

    [Fact]
    public async void Success_hard_delete_volunteer()
    {
        //array
        SeedVolunteers(1);
        var volunteer = _writeDbContext.Volunteers.First();
        HardDeleteVolunteerCommand command = new HardDeleteVolunteerCommand(volunteer.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
