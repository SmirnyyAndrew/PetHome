using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.IntegrationTests.IntegrationFactories;
using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.CreatePet;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using Xunit;

namespace PetHome.IntegrationTests.Features.Volunteer.Write.PetManegment.CreatePet;
public class CreatePetUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<Pet, CreatePetCommand> _sut;

    public CreatePetUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<Pet, CreatePetCommand>>();
    }

    [Fact]
    public async void Success_create_pet()
    {
        //array
        await SeedVolunteersWithAggregates();
        var volunteer = _volunteerWriteDbContext.Volunteers.First();
        var breed = _speciesReadDbContext.Species.SelectMany(b => b.Breeds).First();

        PetMainInfoDto dto = new PetMainInfoDto(
            "Новая кличка",
            breed.SpeciesId,
            "Описание",
            breed.Id,
            "чёрный",
            //shelterId,
            Guid.Empty,
            20d,
            false,
            DateTime.UtcNow,
            false,
            PetStatusEnum.isTreatment,
            DateTime.UtcNow,
            new List<RequisitesesDto>());
        CreatePetCommand command = new CreatePetCommand(volunteer.Id, dto);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.IsSuccess);
    }

}
