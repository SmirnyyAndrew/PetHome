using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
using PetHome.Application.Interfaces.FeatureManagment;
using Xunit;

namespace PetHome.IntegrationTests.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;
public class UpdateMainInfoVolunteerUseCaseTest : BaseTest, IClassFixture<IntegrationTestFactory>
{
    private readonly ICommandHandler<Guid, UpdateMainInfoVolunteerCommand> _sut;
    public UpdateMainInfoVolunteerUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateMainInfoVolunteerCommand>>();
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
