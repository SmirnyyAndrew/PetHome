using PetManagementService.Application.Database.Dto;
using PetManagementService.Application.Features.GraphQL.PetAggregate;
using PetManagementService.Application.Features.GraphQL.VolunteerAggregate;

namespace PetManagementService.Application.Features.GraphQL.Query;
public class PetManagementGraphQLQuery
{
    [UsePaging(typeof(PetGraphQLType))]
    [HotChocolate.Data.UseFiltering]
    public IQueryable<PetDto> Pets([Service] PetGraphQLService petService) 
        => petService.GetAll();


    [UsePaging(typeof(VolunteerGraphQLType))]
    [HotChocolate.Data.UseFiltering]
    public IQueryable<VolunteerDto> Volunteers([Service] VolunteerGraphQLService volunteerService) 
        => volunteerService.GetAll();
}
