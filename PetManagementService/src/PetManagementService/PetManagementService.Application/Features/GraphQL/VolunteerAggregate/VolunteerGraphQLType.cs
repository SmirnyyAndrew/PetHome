using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.GraphQL.VolunteerAggregate;
public class VolunteerGraphQLType : ObjectType<VolunteerDto>
{
    protected override void Configure(IObjectTypeDescriptor<VolunteerDto> descriptor)
    {
        descriptor.Field<PetResolver>(r => r.GetPets(default!, default!));
    }

    public class PetResolver
    {
        public IEnumerable<PetDto> GetPets(
            [Parent] VolunteerDto volunteer,
            [Service] IPetManagementReadDbContext readDbContext)
                => readDbContext.Pets;
    }
}
