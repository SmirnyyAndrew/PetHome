﻿using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
using VolunteerRequestService.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.VolunteerRequests.IntegrationTests.Features.Write.CreateVolunteerRequest;
public class CreateVolunteerRequestUseCaseTest : VolunteerRequestFactory
{
    private readonly ICommandHandler<CreateVolunteerRequestCommand> _sut;

    public CreateVolunteerRequestUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateVolunteerRequestCommand>>();
    }


    [Fact]
    public async void Create_volunteer_request()
    {
        //array  
        Guid userId = Guid.NewGuid();
        string volunteerInfo = "It's length enough volunteer requiest info";
        CreateVolunteerRequestCommand command = new CreateVolunteerRequestCommand(userId, volunteerInfo);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
