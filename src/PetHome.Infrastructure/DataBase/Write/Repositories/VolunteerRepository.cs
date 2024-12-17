using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;
using PetHome.Infrastructure.DataBase.Write.DBContext;

namespace PetHome.Infrastructure.DataBase.Write.Repositories;
public class VolunteerRepository : IVolunteerRepository
{
    private readonly WriteDBContext _dBContext;

    public VolunteerRepository(WriteDBContext dBContext)
    {
        _dBContext = dBContext;
    }

    //Создание волонтёра
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken ct = default)
    {
        await _dBContext.Volunteers.AddAsync(volunteer, ct);
        return volunteer.Id;
    }

    //Изменение волонтёра
    public async Task<Guid> Update(Volunteer volunteer, CancellationToken ct = default)
    {
        _dBContext.Volunteers.Update(volunteer);
        return volunteer.Id;
    }

    //Найти волонтера по ID
    public async Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken ct = default)
    {
        var volunteer = await _dBContext.Volunteers
            .Where(v => v.Id == id)
            .Include(p => p.Pets)
            .ThenInclude(d => d.Medias)
            .FirstOrDefaultAsync(ct);
        if (volunteer == null)
            return Errors.NotFound($"Волонтёр с id = {id}");

        return volunteer;
    }

    //Удаление волонтера
    public async Task<Guid> Remove(Volunteer volunteer, CancellationToken ct = default)
    {
        _dBContext.Remove(volunteer);
        return volunteer.Id;
    }

    //Удаление волонтера по id
    public async Task<Result<bool, Error>> RemoveById(VolunteerId id, CancellationToken ct = default)
    {
        var result = GetById(id, ct);
        if (result.Result.IsFailure)
            return result.Result.Error;

        await Remove(result.Result.Value, ct);
        return true;
    }




    //Удалить коллекцию 
    public void Remove(IEnumerable<Volunteer> volunteers)
    {
        _dBContext.RemoveRange(volunteers);
    }

    public IReadOnlyList<Volunteer> GetDeleted(CancellationToken ct)
    {
        return _dBContext.Volunteers
            .Where(x => x._isDeleted == true)
            .ToList();
    }
}
