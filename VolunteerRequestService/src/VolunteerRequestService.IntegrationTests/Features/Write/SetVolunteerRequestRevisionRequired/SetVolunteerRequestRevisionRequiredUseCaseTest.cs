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
        await SeedVolunteerRequests(1);
        var volunteerRequest = _writeDbContext.VolunteerRequests.First();

        SetVolunteerRequestRevisionRequiredCommand command = new SetVolunteerRequestRevisionRequiredCommand(
            VolunteerRequestId: volunteerRequest.Id,
            AdminId: Guid.NewGuid(),
            RejectedComment: "Unfortunately, your application does not meet our current requirements."
        );

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}