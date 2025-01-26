using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;
using PetHome.VolunteerRequests.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.VolunteerRequests.IntegrationTests.Features.Write.SetVolunteerRequestApproved;
public class SetVolunteerRequestApprovedUseCaseTest : VolunteerRequestFactory
{
    private readonly ICommandHandler<SetVolunteerRequestApprovedCommand> _sut;

    public SetVolunteerRequestApprovedUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SetVolunteerRequestApprovedCommand>>();
    }

    [Fact]
    public async void Set_volunteer_request_approved()
    {
        //array 
        VolunteerRequestId volunteerRequestId = await _createVolunteerRequestContract.Execute(CancellationToken.None);
        RoleId roleId = _getRoleContract.Execute("admin", CancellationToken.None).Result.Value;
        UserId adminId = await _createUserContract.Execute(roleId, CancellationToken.None);
        SetVolunteerRequestApprovedCommand command = new SetVolunteerRequestApprovedCommand(volunteerRequestId, adminId);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
