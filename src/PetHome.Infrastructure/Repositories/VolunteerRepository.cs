using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Volunteers;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Infrastructure.Repositories;
public class VolunteerRepository: IVolunteerRepository
{
    private readonly ApplicationDBContext DBContext;

    public VolunteerRepository(ApplicationDBContext dBContext)
    {
        DBContext = dBContext;
    }

    //Создание волонтёра
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken ct = default)
    {
        await DBContext.Volunteers.AddAsync(volunteer, ct);
        await DBContext.SaveChangesAsync(ct); 
        return volunteer.Id;
    }


    //Найти волонтера по ID
    public async Task<Volunteer> GetById(VolunteerId id, CancellationToken ct = default)
    {
        var volunteer = await DBContext.Volunteers
            .Where(v => v.Id == id)
            .Include(x => x.PetList)
            .FirstAsync(ct);

        return volunteer;
    }

    //Удаление волонтера
    public async Task<Guid> Remove(Volunteer volunteer, CancellationToken ct = default)
    {
        DBContext.Remove(volunteer);
        await DBContext.SaveChangesAsync(ct);

        return volunteer.Id;
    }

    //Удаление волонтера по id
    public async Task<bool> RemoveById(VolunteerId id, CancellationToken ct = default)
    {
        Volunteer volunteer = GetById(id).Result;

        if (volunteer == null)
            return false;

        await Remove(volunteer, ct);
        return true;
    }
}
