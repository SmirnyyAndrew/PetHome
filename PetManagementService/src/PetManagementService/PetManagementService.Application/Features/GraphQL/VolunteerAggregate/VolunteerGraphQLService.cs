using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.GraphQL.VolunteerAggregate;
public class VolunteerGraphQLService(IPetManagementReadDbContext readDbContext)
{
    public IQueryable<VolunteerDto> GetAll() => readDbContext.Volunteers;

    public VolunteerDto? GetById(Guid volunteerId) 
        => readDbContext.Volunteers.FirstOrDefault(v=>v.Id == volunteerId);
}
