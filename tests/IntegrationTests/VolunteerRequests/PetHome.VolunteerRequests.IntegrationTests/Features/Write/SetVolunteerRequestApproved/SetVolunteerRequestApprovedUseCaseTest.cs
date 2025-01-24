using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
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
        Guid volunteerrequestId = Guid.NewGuid();
        Guid adminId = Guid.NewGuid();

        SetVolunteerRequestApprovedCommand command = new SetVolunteerRequestApprovedCommand(volunteerrequestId, adminId);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
