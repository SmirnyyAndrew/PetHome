using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Application.Features.Volunteers;

//Инверсия управления
public interface IVolunteerRepository
{
    //Создание волонтёра
    public Task<Guid> Add(Volunteer volunteer, CancellationToken ct = default);

    //Найти волонтера по ID
    public Task<Volunteer> GetById(VolunteerId id, CancellationToken ct = default);

    //Удаление волонтера
    public Task<Guid> Remove(Volunteer volunteer, CancellationToken ct = default);

    //Удаление волонтера по id
    public Task<bool> RemoveById(VolunteerId id, CancellationToken ct = default);
}
