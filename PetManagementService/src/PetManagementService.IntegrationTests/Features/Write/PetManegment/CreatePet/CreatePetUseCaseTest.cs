using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Dto.Pet;
using PetManagementService.Application.Features.Write.PetManegment.CreatePet;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetManagementService.IntegrationTests.Features.Write.PetManegment.CreatePet;

[Collection("Pet")]
public class CreatePetUseCaseTest : PetManagementFactory
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
        var volunteer = _writeDbContext.Volunteers
            .ToList()
            .First();
        var breed = _writeDbContext.Species.SelectMany(b => b.Breeds).First();

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
