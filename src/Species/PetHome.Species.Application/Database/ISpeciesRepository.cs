using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;
using Species_ = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Species.Application.Database;
public interface ISpeciesRepository
{
    //Добавить вид животного
    public Task<Result<Guid, Error>> Add(Species_ species, CancellationToken ct);
    
    //Добавить коллекцию вид животного
    public Task<UnitResult<Error>> AddRange(IEnumerable<Species_> species, CancellationToken ct);

    //Получить вид животного по id
    public Task<Result<Species_, Error>> GetById(Guid id, CancellationToken ct);

    //Получить вид животного по имени
    public Task<Result<Species_, Error>> GetByName(string name, CancellationToken ct);

    //Удалить один элемент
    public Task<Guid> Remove(Species_ species, CancellationToken ct = default);

    //Удалить один элемент по id
    public Task<bool> RemoveById(Guid id, CancellationToken ct = default);

    //Удалить коллекцию 
    public void Remove(IEnumerable<Species_> species);

    //Обновить
    public Task<Guid> Update(Species_ species, CancellationToken ct);

    //Обновление коллекции вида
    public Task<UnitResult<Error>> UpdateRange(IEnumerable<Species_> species, CancellationToken ct);
}
