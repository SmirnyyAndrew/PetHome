using CSharpFunctionalExtensions;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.RepositoryInterfaces;
public interface ISpeciesRepository
{
    //Добавить вид животного
    public Task<Result<Guid, Error>> Add(Species species, CancellationToken ct);

    //Получить вид животного по id
    public Task<Result<Species, Error>> GetById(Guid id, CancellationToken ct);

    //Получить вид животного по имени
    public Task<Result<Species, Error>> GetByName(string name, CancellationToken ct);

    //Удалить один элемент
    public Task<Guid> Remove(Species species, CancellationToken ct = default);

    //Удалить один элемент по id
    public Task<bool> RemoveById(Guid id, CancellationToken ct = default);

    //Удалить коллекцию 
    public void Remove(IEnumerable<Species> species);

    //Обновить
    public Task<Guid> Update(Species species, CancellationToken ct);
}
