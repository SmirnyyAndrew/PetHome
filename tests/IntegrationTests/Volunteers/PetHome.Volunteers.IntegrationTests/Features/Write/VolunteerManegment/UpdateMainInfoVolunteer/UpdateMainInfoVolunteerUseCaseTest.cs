using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
using PetHome.Volunteers.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Volunteers.IntegrationTests.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<Guid, UpdateMainInfoVolunteerCommand> _sut;
    public UpdateMainInfoVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateMainInfoVolunteerCommand>>();
    }

    [Fact]
    public async void Success_update_volunteer_main_info()
    {
        //array
        await SeedVolunteers(1);
        var volunteer = _writeDbContext.Volunteers.First();

        FullNameDto fullNameDto = new FullNameDto("Смирнов", "Иван");
        List<string> phoneNumber = new List<string>() { "89333333333", "83773989333" };
        UpdateMainInfoVolunteerDto updateMainInfoVolunteerDto = new UpdateMainInfoVolunteerDto(
            fullNameDto,
            "Описание",
            phoneNumber,
            "mail@gmail.com");
        UpdateMainInfoVolunteerCommand command = new UpdateMainInfoVolunteerCommand(
            volunteer.Id,
            updateMainInfoVolunteerDto);

        //act 
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
