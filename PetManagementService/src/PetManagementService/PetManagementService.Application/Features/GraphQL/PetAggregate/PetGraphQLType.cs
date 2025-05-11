using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.GraphQL.PetAggregate;
public class PetGraphQLType : ObjectType<PetDto>
{
    protected override void Configure(IObjectTypeDescriptor<PetDto> descriptor)
    { 
        descriptor.Field<VolunteerResolver>(r => r.GetVolunteer(default, default));
    }

    public class VolunteerResolver
    {
        public VolunteerDto? GetVolunteer(
            [Parent] PetDto petDto,
            [Service] IPetManagementReadDbContext readDbContext)
                => readDbContext.Volunteers.FirstOrDefault(v => v.Id == petDto.VolunteerId);
    } 
}
