using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Contracts.Contracts;
using PetHome.Volunteers.Contracts.Dto;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Contracts;
public class GetVolunteerInformationUsingContract(IVolunteerRepository repository)
    : IGetVolunteerInformationContract
{
    public async Task<VolunteerDto?> Execute(Guid Id, CancellationToken ct)
    {
        var getVolunteerResult = await repository.GetById(Id,ct);
        if(getVolunteerResult.IsFailure) 
            return null;

        Volunteer volunteer = getVolunteerResult.Value;
        VolunteerDto? volunteerDto = new VolunteerDto(
            volunteer.Id,
            volunteer.UserId,
            volunteer.FullName?.FirstName,
            volunteer.FullName?.LastName,
            volunteer.Email,
            volunteer.Description,
            volunteer.StartVolunteeringDate.Value);
        return volunteerDto;
    }
}
