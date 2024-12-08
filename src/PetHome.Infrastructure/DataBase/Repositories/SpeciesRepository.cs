using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Infrastructure.DataBase.Repositories;
public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDBContext _dbContext;
    public SpeciesRepository(ApplicationDBContext dBContext)
    {
        _dbContext = dBContext;
    }


    //Добавить вид животного
    public async Task<Result<Guid, Error>> Add(Species species, CancellationToken ct)
    {
        await _dbContext.Specieses.AddAsync(species, ct);
        await _dbContext.SaveChangesAsync(ct);
        return species.Id.Value;
    }


    //Получить вид животного по id
    public async Task<Result<Species, Error>> GetById(Guid id, CancellationToken ct)
    {
        SpeciesId speciesId = SpeciesId.Create(id).Value;
        Species species = await _dbContext.Specieses
            .Include(x => x.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);
        if (species == null)
            return Errors.NotFound($"Вид животного с id {id} не найден");

        return species;
    }

    //Получить вид животного по имени
    public async Task<Result<Species, Error>> GetByName(string name, CancellationToken ct)
    {
        var result = _dbContext.Specieses
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
        await _dbContext.SaveChangesAsync(ct);

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
        _dbContext.SaveChanges();
    }

    //Обновление вида
    public async Task<Guid> Update(Species species, CancellationToken ct)
    {
        _dbContext.Specieses.Update(species);
        await _dbContext.SaveChangesAsync(ct);
        return species.Id;
    }

}
