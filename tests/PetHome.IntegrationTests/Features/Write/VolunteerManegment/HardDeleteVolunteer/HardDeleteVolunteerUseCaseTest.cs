using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
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
        var volunteer = _writeDbContext.Volunteers.First();
        HardDeleteVolunteerCommand command = new HardDeleteVolunteerCommand(volunteer.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
