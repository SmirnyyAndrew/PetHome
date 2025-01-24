using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestSubmitted;
using PetHome.VolunteerRequests.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.VolunteerRequests.IntegrationTests.Features.Write.SetVolunteerRequestSubmitted;

public class SetVolunteerRequestSubmittedUseCaseTest : VolunteerRequestFactory
{
    private readonly ICommandHandler<SetVolunteerRequestSubmittedCommand> _sut;

    public SetVolunteerRequestSubmittedUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SetVolunteerRequestSubmittedCommand>>();
    }

    [Fact]
    public async void Set_volunteer_request_submitted()
    {
        //array 
        VolunteerRequestId volunteerRequestId = await _createVolunteerRequestContract.Execute(CancellationToken.None);
        RoleId roleId = await _getRoleContract.Execute("admin", CancellationToken.None);
        UserId adminId = await _createUserContract.Execute(roleId, CancellationToken.None);
        
        SetVolunteerRequestSubmittedCommand command = new SetVolunteerRequestSubmittedCommand(
            volunteerRequestId, 
            adminId);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}