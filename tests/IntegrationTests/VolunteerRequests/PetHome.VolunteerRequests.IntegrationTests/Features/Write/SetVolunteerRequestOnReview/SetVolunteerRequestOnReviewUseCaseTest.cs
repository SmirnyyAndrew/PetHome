using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.RolePermission;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestOnReview;
using PetHome.VolunteerRequests.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.VolunteerRequests.IntegrationTests.Features.Write.SetVolunteerRequestOnReview;
public class SetVolunteerRequestOnReviewUseCaseTest : VolunteerRequestFactory
{
    private readonly ICommandHandler<SetVolunteerRequestOnReviewCommand> _sut;

    public SetVolunteerRequestOnReviewUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SetVolunteerRequestOnReviewCommand>>();
    }

    [Fact]
    public async void Set_volunteer_request_on_review()
    {
        //array 
        var createVolunteerRequestIdResult = await _createVolunteerRequestContract.Execute(CancellationToken.None);
        RoleId roleId = _getRoleContract.Execute("admin", CancellationToken.None).Result.Value;
        var createAdminId = await _createUserContract.Execute(roleId, CancellationToken.None);

        var createFirstUserId = await _createUserContract.Execute(roleId, CancellationToken.None);
        var createSecondUserId = await _createUserContract.Execute(roleId, CancellationToken.None);
        List<UserId> userIds = new() { createFirstUserId.Value, createSecondUserId.Value};

        var discussionId = await _createDiscussionContract.Execute(userIds, CancellationToken.None);
        SetVolunteerRequestOnReviewCommand command = new SetVolunteerRequestOnReviewCommand(
            createVolunteerRequestIdResult.Value,
            createAdminId.Value, 
            discussionId.Value);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
