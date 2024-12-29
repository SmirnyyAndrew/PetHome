using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Species.Infrastructure.Database.Write.Repositories;
public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDBContext _dbContext;
    public SpeciesRepository(WriteDBContext dBContext)
    {
        _dbContext = dBContext;
    }


    //Добавить вид животного
    public async Task<Result<Guid, Error>> Add(Species species, CancellationToken ct)
    {
        await _dbContext.Species.AddAsync(species, ct);
        return species.Id.Value;
    }

    //Добавить коллекцию вид животного
    public async Task<UnitResult<Error>> AddRange(IEnumerable<Species> species, CancellationToken ct)
    {
        await _dbContext.Species.AddRangeAsync(species, ct);
        return Result.Success<Error>();
    }

    //Получить вид животного по id
    public async Task<Result<Species, Error>> GetById(Guid id, CancellationToken ct)
    {
        SpeciesId speciesId = SpeciesId.Create(id).Value;
        Species species = await _dbContext.Species
            .Include(x => x.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);
        if (species == null)
            return Errors.NotFound($"Вид животного с id {id} не найден");

        return species;
    }

    //Получить вид животного по имени
    public async Task<Result<Species, Error>> GetByName(string name, CancellationToken ct)
    {
        var result = _dbContext.Species
                 .Include(x => x.Breeds)
                 .TryFirst(s => s.Name.Value.ToLower() == name.ToLower());
        if (result == null)
            return Errors.NotFound($"Вид животного с именем {name} не найден");

        return result.Value;
    }

    //Удалить один элемент
    public async Task<Guid> Remove(Species species, CancellationToken ct = default)
    {
        _dbContext.Remove(species);
        return species.Id;
    }

    //Удалить один элемент по id
    public async Task<bool> RemoveById(Guid id, CancellationToken ct = default)
    {
        Species species = GetById(id, ct).Result.Value;
        await Remove(species, ct);
        return true;
    }

    //Удалить коллекцию 
    public void Remove(IEnumerable<Species> species)
    {
        _dbContext.RemoveRange(species);
    }

    //Обновление вида
    public async Task<Guid> Update(Species species, CancellationToken ct)
    {
        _dbContext.Species.Update(species);
        return species.Id;
    }

    //Обновление коллекции вида
    public async Task<UnitResult<Error>> UpdateRange(IEnumerable<Species> species, CancellationToken ct)
    {
        _dbContext.Species.UpdateRange(species);
        return Result.Success<Error>();
    }

}
