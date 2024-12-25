using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Features.Write.PetManegment.ChangePetInfo;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.IntegrationTests.Features.Write.PetManegment.ChangePetInfo;
public class ChangePetInfoUseCaseTest : BaseFactory
{
    private readonly ICommandHandler<string, ChangePetInfoCommand> _sut;
    public ChangePetInfoUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        var scope = factory.Services.CreateScope();
        _sut = scope.ServiceProvider.GetRequiredService<ICommandHandler<string, ChangePetInfoCommand>>();
    }

    [Fact]
    public async void Success_change_pet_info()
    {
        //array
        await SeedVolunteersWithAggregates();  

        var breed = _readDbContext.Species
            .SelectMany(b => b.Breeds)
            .First(); 
        var pet = _writeDbContext.Volunteers
            .Include(p => p.Pets)
            .SelectMany(p => p.Pets)
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
