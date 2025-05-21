using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.User;
using PetManagementService.Application.Features.Write.VolunteerManegment.CreateVolunteer;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;
namespace PetManagementService.IntegrationTests.Features.Write.VolunteerManegment.CreateVolunteer;

[Collection("Volunteer")]
public class CreateVolunteerUseCaseTest : PetManagementFactory
{
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _sut;
    public CreateVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async void Create_volunteer()
    {
        //array
        FullNameDto fullNameDto = new FullNameDto("Иван", "Иванов");
        List<string> phoneNumbers = new List<string>() { "89383838733", "89332232332", "89777772332" };
        List<SocialNetworkDto> socialNetworks = new List<string>() { "vk.com/2943832", "tg/291221" }
        .Select(s => new SocialNetworkDto(s)).ToList();

        List<RequisitesesDto> requisiteses = new List<RequisitesesDto>();
        List<CertificateDto> certificates = new List<CertificateDto>();
        CreateVolunteerCommand command = new CreateVolunteerCommand(
            fullNameDto,
            "mail@mail.ru",
            "Описание",
            "username",
            DateTime.UtcNow,
            phoneNumbers,
            socialNetworks,
            certificates,
            requisiteses,
            Guid.NewGuid());

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
