using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Write.PetManegment.CreatePet;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using Xunit;

namespace PetHome.IntegrationTests.Features.Write.PetManegment.CreatePet;
public class CreatePetUseCaseTest : BaseTest, IClassFixture<IntegrationTestFactory>
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
        var volunteer = _writeDbContext.Volunteers.First();
        var breed = _writeDbContext.Species.SelectMany(b => b.Breeds).First(); 

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
    }

}
