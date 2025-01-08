using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

namespace PetHome.Volunteers.Infrastructure.Database.Write.Repositories;
public class VolunteerRepository : IVolunteerRepository
{
    private readonly VolunteerWriteDbContext _dBContext;

    public VolunteerRepository(VolunteerWriteDbContext dBContext)
    {
        _dBContext = dBContext;
    }

    //Создание волонтёра
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken ct = default)
    {
        await _dBContext.Volunteers.AddAsync(volunteer, ct);
        return volunteer.Id;
    }


    //Создание коллекции волонтёра
    public async Task<UnitResult<Error>> AddRange(IEnumerable<Volunteer> volunteers, CancellationToken ct)
    {
        await _dBContext.Volunteers.AddRangeAsync(volunteers, ct);
        return Result.Success<Error>();
    }

    //Изменение волонтёра
    public async Task<Guid> Update(Volunteer volunteer, CancellationToken ct = default)
    {
        _dBContext.Volunteers.Update(volunteer);
        return volunteer.Id;
    }

    //Изменение волонтёров
    public async Task<UnitResult<Error>> UpdateRange(IEnumerable<Volunteer> volunteers, CancellationToken ct = default)
    {
        _dBContext.Volunteers.UpdateRange(volunteers);
        return Result.Success<Error>();
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
            .Where(x => x.IsDeleted == true)
            .ToList();
    }
}
