using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
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
    public async void Success_set_volunteer_request_on_review()
    {
        //array 
        Guid adminId = Guid.NewGuid();
        Guid volunteerRequestId = Guid.NewGuid();
        SetVolunteerRequestSubmittedCommand command = new SetVolunteerRequestSubmittedCommand(volunteerRequestId, adminId);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}