using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Database;
public interface IPetManagementReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
    IQueryable<SpeciesDto> Species { get; }
}
