using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;
using VolunteerRequestService.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.VolunteerRequests.IntegrationTests.Features.Write.SetVolunteerRequestRevisionRequired;

public class SetVolunteerRequestRevisionRequiredUseCaseTest : VolunteerRequestFactory
{
    private readonly ICommandHandler<SetVolunteerRequestRevisionRequiredCommand> _sut;

    public SetVolunteerRequestRevisionRequiredUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SetVolunteerRequestRevisionRequiredCommand>>();
    }

    [Fact]
    public async void Set_volunteer_request_revision_required()
    {
        //array 
        var createVolunteerRequestIdResult = await _createVolunteerRequestContract.Execute(CancellationToken.None);
        RoleId roleId = _getRoleContract.Execute("admin", CancellationToken.None).Result.Value;
        var createAdminId = await _createUserContract.Execute(roleId, CancellationToken.None);
        string message = "message";

        SetVolunteerRequestRevisionRequiredCommand command = new SetVolunteerRequestRevisionRequiredCommand(
            createVolunteerRequestIdResult.Value,
            createAdminId.Value,
            message);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}