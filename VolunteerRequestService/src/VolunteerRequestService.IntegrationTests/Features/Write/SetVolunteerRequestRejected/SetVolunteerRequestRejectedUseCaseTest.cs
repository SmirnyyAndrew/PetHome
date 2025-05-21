using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
using VolunteerRequestService.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.VolunteerRequests.IntegrationTests.Features.Write.SetVolunteerRequestRejected;

public class SetVolunteerRequestRejectedUseCaseTest : VolunteerRequestFactory
{
    private readonly ICommandHandler<SetVolunteerRequestRejectedCommand> _sut;

    public SetVolunteerRequestRejectedUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SetVolunteerRequestRejectedCommand>>();
    }

    [Fact]
    public async void Set_volunteer_request_rejected()
    {
        //array 
        var createVolunteerRequestIdResult = await _createVolunteerRequestContract.Execute(CancellationToken.None);
        RoleId roleId = _getRoleContract.Execute("admin", CancellationToken.None).Result.Value;
        var createAdminId = await _createUserContract.Execute(roleId, CancellationToken.None);
        string rejectedMessage = "message";

        SetVolunteerRequestRejectedCommand command = new SetVolunteerRequestRejectedCommand(
            createVolunteerRequestIdResult.Value, createAdminId.Value, rejectedMessage);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}