using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Database;

//Инверсия управления
public interface IVolunteerRepository
{
    //Создание волонтёра
    public Task<Guid> Add(Volunteer volunteer, CancellationToken ct);
   
    //Создание коллекции волонтёра
    public Task<UnitResult<Error>> AddRange(IEnumerable<Volunteer> volunteers, CancellationToken ct);

    //Изменение волонтёра
    public Task<Guid> Update(Volunteer volunteer, CancellationToken ct);//Изменение волонтёров

    //Изменение волонтёров
    public Task<UnitResult<Error>> UpdateRange(IEnumerable<Volunteer> volunteers, CancellationToken ct);
 

    //Найти волонтера по ID
    public Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken ct);

    //Удаление волонтера
    public Task<Guid> Remove(Volunteer volunteer, CancellationToken ct);

    //Удаление волонтера по id
    public Task<Result<bool, Error>> RemoveById(Guid id, CancellationToken ct);

    //Удалить коллекцию
    public void Remove(IEnumerable<Volunteer> volunteers);

    //Получить список Волонётров у кого параметр isDeleted == true
    public IReadOnlyList<Volunteer> GetDeleted(CancellationToken ct);
}
