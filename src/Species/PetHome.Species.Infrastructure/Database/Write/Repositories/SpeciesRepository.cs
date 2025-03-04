using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Species.Application.Database;
using PetHome.Species.Infrastructure.Database.Write.DBContext;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Species.Infrastructure.Database.Write.Repositories;
public class SpeciesRepository : ISpeciesRepository
{
    private readonly SpeciesWriteDbContext _dbContext;
    public SpeciesRepository(SpeciesWriteDbContext dBContext)
    {
        _dbContext = dBContext;
    }


    //Добавить вид животного
    public async Task<Result<Guid, Error>> Add(_Species species, CancellationToken ct)
    {
        await _dbContext.Species.AddAsync(species, ct);
        return species.Id.Value;
    }

    //Добавить коллекцию вид животного
    public async Task<UnitResult<Error>> AddRange(IEnumerable<_Species> species, CancellationToken ct)
    {
        await _dbContext.Species.AddRangeAsync(species, ct);
        return Result.Success<Error>();
    }

    //Получить вид животного по id
    public async Task<Result<_Species, Error>> GetById(Guid id, CancellationToken ct)
    {
        SpeciesId speciesId = SpeciesId.Create(id).Value;
        _Species species = await _dbContext.Species
            .Include(x => x.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);
        if (species == null)
            return Errors.NotFound($"Вид животного с id {id}");

        return species;
    }

    //Получить вид животного по имени
    public async Task<Result<_Species, Error>> GetByName(string name, CancellationToken ct)
    {
        var result = await _dbContext.Species
                 .Include(x => x.Breeds)
                 .FirstOrDefaultAsync(s => s.Name == name, ct);
        if (result == null)
            return Errors.NotFound($"Вид животного с именем {name}");

        return result;
    }

    //Удалить один элемент
    public async Task<Guid> Remove(_Species species, CancellationToken ct = default)
    {
        _dbContext.Remove(species);
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
    public void Remove(IEnumerable<_Species> species)
    {
        _dbContext.RemoveRange(species);
    }

    //Обновление вида
    public async Task<Guid> Update(_Species species, CancellationToken ct)
    {
        _dbContext.Species.Update(species);
        return species.Id;
    }

    //Обновление коллекции вида
    public async Task<UnitResult<Error>> UpdateRange(IEnumerable<_Species> species, CancellationToken ct)
    {
        _dbContext.Species.UpdateRange(species);
        return Result.Success<Error>();
    }

}
