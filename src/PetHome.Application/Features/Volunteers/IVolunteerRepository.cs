using CSharpFunctionalExtensions;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers;

//Инверсия управления
public interface IVolunteerRepository
{
    //Создание волонтёра
    public Task<Guid> Add(Volunteer volunteer, CancellationToken ct = default);
    //Создание волонтёра
    public Task<Guid> Update(Volunteer volunteer, CancellationToken ct = default);

    //Найти волонтера по ID
    public Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken ct = default);

    //Удаление волонтера
    public Task<Guid> Remove(Volunteer volunteer, CancellationToken ct = default);

    //Удаление волонтера по id
    public Task<bool> RemoveById(VolunteerId id, CancellationToken ct = default);
}
