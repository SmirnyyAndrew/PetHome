using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
using PetHome.Volunteers.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Volunteers.IntegrationTests.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public class SoftDeleteVolunteerUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<Guid, SoftDeleteRestoreVolunteerCommand> _sut;
    public SoftDeleteVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SoftDeleteRestoreVolunteerCommand>>();
    }

    [Fact]
    public async void Soft_delete_restore_volunteer()
    {
        //array
        await SeedVolunteers(1);
        var volunteer = _writeDbContext.Volunteers.First();
        SoftDeleteRestoreVolunteerCommand command = new SoftDeleteRestoreVolunteerCommand(volunteer.Id);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
