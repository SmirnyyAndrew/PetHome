using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;
using PetManagementService.Infrastructure.Database.Write.DBContext;

namespace PetManagementService.Infrastructure.Database.Write.Repositories;
public class SpeciesRepository(PetManagementWriteDBContext dbContext) 
    : ISpeciesRepository
{
    //Добавить вид животного
    public async Task<Result<Guid, Error>> Add(Species species, CancellationToken ct)
    {
        await dbContext.Species.AddAsync(species, ct);
        return species.Id.Value;
    }

    //Добавить коллекцию вид животного
    public async Task<UnitResult<Error>> AddRange(IEnumerable<Species> species, CancellationToken ct)
    {
        await dbContext.Species.AddRangeAsync(species, ct);
        return Result.Success<Error>();
    }

    //Получить вид животного по id
    public async Task<Result<Species, Error>> GetById(Guid id, CancellationToken ct)
    {
        SpeciesId speciesId = SpeciesId.Create(id).Value;
        Species species = await dbContext.Species
            .Include(x => x.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);
        if (species == null)
            return Errors.NotFound($"Вид животного с id {id}");

        return species;
    }

    //Получить вид животного по имени
    public async Task<Result<Species, Error>> GetByName(string name, CancellationToken ct)
    {
        var result = await dbContext.Species
                 .Include(x => x.Breeds)
                 .FirstOrDefaultAsync(s => s.Name == name, ct);
        if (result == null)
            return Errors.NotFound($"Вид животного с именем {name}");

        return result;
    }

    //Удалить один элемент
    public async Task<Guid> Remove(Species species, CancellationToken ct = default)
    {
        dbContext.Remove(species);
        return species.Id;
    }

    //Удалить один элемент по id
    public async Task<bool> RemoveById(Guid id, CancellationToken ct = default)
    { 
        var getSpeciesResult  = await GetById(id, ct);
        if (getSpeciesResult.IsFailure)
            return false;

        await Remove(getSpeciesResult.Value, ct);
        return true;
    }

    //Удалить коллекцию 
    public void Remove(IEnumerable<Species> species)
    {
        dbContext.RemoveRange(species);
    }

    //Обновление вида
    public async Task<Guid> Update(Species species, CancellationToken ct)
    {
        dbContext.Species.Update(species);
        return species.Id;
    }

    //Обновление коллекции вида
    public async Task<UnitResult<Error>> UpdateRange(IEnumerable<Species> species, CancellationToken ct)
    {
        dbContext.Species.UpdateRange(species);
        return Result.Success<Error>();
    }

}
