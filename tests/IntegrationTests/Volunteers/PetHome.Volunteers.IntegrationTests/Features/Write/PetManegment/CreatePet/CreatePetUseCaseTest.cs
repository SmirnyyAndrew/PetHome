using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.CreatePet;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Volunteers.IntegrationTests.Features.Write.PetManegment.CreatePet;
public class CreatePetUseCaseTest : VolunteerFactory
{
    private readonly ICommandHandler<Pet, CreatePetCommand> _sut;

    public CreatePetUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Pet, CreatePetCommand>>();
    }

    [Fact]
    public async void Create_pet()
    {
        //array
        await SeedVolunteersWithAggregates();
        var volunteer = _writeDbContext.Volunteers.First();
        var breed = _readDbContext.Species.SelectMany(b => b.Breeds).First();

        PetMainInfoDto dto = new PetMainInfoDto(
            "Новая кличка",
            breed.SpeciesId,
            "Описание",
              breed.Id,
            "чёрный",
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
