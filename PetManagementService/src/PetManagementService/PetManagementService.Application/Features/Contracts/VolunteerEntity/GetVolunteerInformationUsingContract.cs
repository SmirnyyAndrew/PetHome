using PetManagementService.Application.Database;
using PetManagementService.Contracts.Contracts.VolunteerEntity;
using PetManagementService.Contracts.Dto;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Contracts.VolunteerEntity;
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
