using PetManagementService.Application.Database.Dto;
using PetManagementService.Application.Features.GraphQL.PetAggregate;
using PetManagementService.Application.Features.GraphQL.VolunteerAggregate;

namespace PetManagementService.Application.Features.GraphQL.Query;
public class PetManagementGraphQLQuery
{
    [HotChocolate.Data.UseFiltering]
    [UseSorting]
    public IQueryable<PetDto> Pets([Service] PetGraphQLService petService) 
        => petService.GetAll();

    [HotChocolate.Data.UseFiltering]
    [UseSorting]
    public IQueryable<VolunteerDto> Volunteers([Service] VolunteerGraphQLService volunteerService) 
        => volunteerService.GetAll();
}
