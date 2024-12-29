using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using Xunit;
namespace PetHome.IntegrationTests.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public class HardDeleteVolunteerUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<bool, HardDeleteVolunteerCommand> _sut;
    public HardDeleteVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<bool, HardDeleteVolunteerCommand>>();
    }

    [Fact]
    public async void Success_hard_delete_volunteer()
    {
        //array
        await SeedVolunteers(1);
        var volunteer = _volunteerWriteDbContext.Volunteers.First();
        HardDeleteVolunteerCommand command = new HardDeleteVolunteerCommand(volunteer.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
