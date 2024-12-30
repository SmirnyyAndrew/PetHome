using PetHome.Species.Application.Database.Dto;
using PetHome.Volunteers.Application.Database.Dto;

namespace PetHome.Volunteers.Application.Database;
public interface IVolunteerReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
    IQueryable<SpeciesDto> Species { get; }
}
