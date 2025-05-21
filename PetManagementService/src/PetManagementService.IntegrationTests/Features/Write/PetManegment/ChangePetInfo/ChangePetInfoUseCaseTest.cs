using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Features.Write.PetManegment.ChangePetInfo;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetManagementService.IntegrationTests.Features.Write.PetManegment.ChangePetInfo;

[Collection("Pet")]
public class ChangePetInfoUseCaseTest : PetManagementFactory
{
    private readonly ICommandHandler<string, ChangePetInfoCommand> _sut;
    private readonly IntegrationTestFactory factory;

    //private readonly PetManagementWriteDBContext _writeDbContext;
    public ChangePetInfoUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<string, ChangePetInfoCommand>>();
        this.factory = factory;
    }

    [Fact]
    public async void Change_pet_info()
    {
        //array 
        await SeedVolunteersWithAggregates();

        var breed = _writeDbContext.Species
            .SelectMany(b => b.Breeds)
            .ToList()
            .First();
        var pet = _writeDbContext.Volunteers
            .SelectMany(p => p.Pets)
            .ToList()
            .First();

        List<RequisitesesDto> requisites = new List<RequisitesesDto>();

        ChangePetInfoCommand command = new ChangePetInfoCommand(
             pet.Id,
            "Новая кличка",
             breed.SpeciesId,
            "Описание",
             breed.Id,
            "чёрный",
            pet.ShelterId,
            20d,
            false,
            DateTime.UtcNow,
            false,
            PetStatusEnum.isTreatment,
            pet.VolunteerId,
            requisites);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert 
        Assert.True(result.IsSuccess);
    }
}
