using PetManagementService.Application.Database.Dto;
using PetManagementService.Application.Features.GraphQL.PetAggregate;
using PetManagementService.Application.Features.GraphQL.VolunteerAggregate;

namespace PetManagementService.Application.Features.GraphQL.Query;
public class PetManagementGraphQLQuery
{ 
    public IQueryable<PetDto> Pets([Service] PetGraphQLService petService) 
        => petService.GetAll();
     
    public IQueryable<VolunteerDto> Volunteers([Service] VolunteerGraphQLService volunteerService) 
        => volunteerService.GetAll();
}
