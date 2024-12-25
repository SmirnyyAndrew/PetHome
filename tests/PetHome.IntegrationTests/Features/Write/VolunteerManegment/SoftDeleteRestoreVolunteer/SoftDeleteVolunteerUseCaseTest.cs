using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
using PetHome.Application.Interfaces.FeatureManagment;
using Xunit;

namespace PetHome.IntegrationTests.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public class SoftDeleteVolunteerUseCaseTest : BaseTest, IClassFixture<IntegrationTestFactory>
{
    private readonly ICommandHandler<Guid, SoftDeleteRestoreVolunteerCommand> _sut;
    public SoftDeleteVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SoftDeleteRestoreVolunteerCommand>>();
    }

    [Fact]
    public async void Success_soft_delete_restore_volunteer()
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
