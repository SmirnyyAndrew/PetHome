using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.ValueObjects.RolePermission;
using PetHome.SharedKernel.ValueObjects.User;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestOnReview;
using VolunteerRequestService.IntegrationTests.IntegrationFactories;
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
        await SeedVolunteerRequests(1);
        var volunteerRequest = _writeDbContext.VolunteerRequests.First();

         SetVolunteerRequestOnReviewCommand command = new (
            VolunteerRequestId: volunteerRequest.Id,
            AdminId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            DiscussionId: Guid.NewGuid(),
            RelationName: "Friend of the organization"
        );

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
