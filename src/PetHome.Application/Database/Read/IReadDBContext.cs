using Microsoft.EntityFrameworkCore;

namespace PetHome.Application.Database.Read;
public interface IReadDBContext
{
    DbSet<VolunteerDto> Volunteers { get; }
    DbSet<PetDto> Pets { get; }
    DbSet<SpeciesDto> Species { get; }
}
