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
        await SeedVolunteerRequests(1);
        var volunteerRequest = _writeDbContext.VolunteerRequests.First();
         
        SetVolunteerRequestRejectedCommand command = new ( 
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