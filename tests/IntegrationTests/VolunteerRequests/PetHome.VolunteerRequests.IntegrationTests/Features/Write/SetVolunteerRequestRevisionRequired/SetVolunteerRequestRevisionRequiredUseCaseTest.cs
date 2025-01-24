using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;
using PetHome.VolunteerRequests.IntegrationTests.IntegrationFactories;
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
        Guid adminId = Guid.NewGuid();
        Guid volunteerRequestId = Guid.NewGuid();
        string rejectedMessage = "message";
        SetVolunteerRequestRevisionRequiredCommand command = new SetVolunteerRequestRevisionRequiredCommand(volunteerRequestId, adminId, rejectedMessage);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}