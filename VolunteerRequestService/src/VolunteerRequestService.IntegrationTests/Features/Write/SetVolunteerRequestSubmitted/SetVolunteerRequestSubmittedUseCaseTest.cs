using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestSubmitted;
using VolunteerRequestService.IntegrationTests.IntegrationFactories;
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
        await SeedVolunteerRequests(1);
        var volunteerRequest = _writeDbContext.VolunteerRequests.First();
         
        SetVolunteerRequestSubmittedCommand command = new (
            VolunteerRequestId: volunteerRequest.Id,
            AdminId: Guid.NewGuid());

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}