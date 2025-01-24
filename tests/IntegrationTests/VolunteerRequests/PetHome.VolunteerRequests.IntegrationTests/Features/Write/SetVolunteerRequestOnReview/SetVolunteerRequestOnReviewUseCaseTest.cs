using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
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
    public async void Success_set_volunteer_request_on_review()
    {
        //array 
        Guid adminId = Guid.NewGuid();
        Guid volunteerRequestId = Guid.NewGuid();
        Guid discussionId = Guid.NewGuid();
        SetVolunteerRequestOnReviewCommand command = new SetVolunteerRequestOnReviewCommand(volunteerRequestId, adminId, discussionId);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
