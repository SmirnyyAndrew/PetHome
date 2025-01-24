using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
using PetHome.VolunteerRequests.IntegrationTests.IntegrationFactories;
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
        VolunteerRequestId volunteerRequestId = await _createVolunteerRequestContract.Execute(CancellationToken.None);
        RoleId roleId = await _getRoleContract.Execute("admin", CancellationToken.None);
        UserId adminId = await _createUserContract.Execute(roleId, CancellationToken.None);
        string rejectedMessage = "message";
       
        SetVolunteerRequestRejectedCommand command = new SetVolunteerRequestRejectedCommand(volunteerRequestId, adminId, rejectedMessage);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}