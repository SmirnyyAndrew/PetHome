using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetHome.SharedKernel.ValueObjects.User;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;
using VolunteerRequestService.IntegrationTests.IntegrationFactories;
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
        await SeedVolunteerRequests(1);
        var volunteerRequest = _writeDbContext.VolunteerRequests.First(); 

        SetVolunteerRequestApprovedCommand command = new(
            VolunteerRequestId: volunteerRequest.Id,
            AdminId: Guid.NewGuid(),
            Email: "volunteer@example.com",
            UserName: "JohnDoe",
            StartVolunteeringDate: DateTime.UtcNow.AddDays(7),
            Requisites: new List<RequisitesesDto>
            {
                new RequisitesesDto("Bank Transfer", "Transfer to XYZ Bank", PaymentMethodEnum.Card),
                new RequisitesesDto("Cash at Event", "Bring cash on day of volunteering", PaymentMethodEnum.Cash)
            },
            Certificates: new List<CertificateDto>
            {
                new CertificateDto("First Aid", "Certified First Aid Level 1"),
                new CertificateDto("Background Check", "Cleared")
            } 
        );

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
