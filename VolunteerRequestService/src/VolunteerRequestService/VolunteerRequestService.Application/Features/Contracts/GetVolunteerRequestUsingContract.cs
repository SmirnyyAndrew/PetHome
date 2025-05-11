using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Contracts;
using PetHome.VolunteerRequests.Contracts.Dto;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Contracts;
public class GetVolunteerRequestUsingContract(IVolunteerRequestRepository repository) 
    : IGetVolunteerRequestContract
{ 
    public async Task<VolunteerRequestDto?> Execute(Guid volunteerRequestId, CancellationToken ct)
    {
        VolunteerRequest? volunteerRequest = await repository.GetById(volunteerRequestId, ct);
        if (volunteerRequest is null) 
            return null;

        VolunteerRequestDto volunteerRequestDto = new VolunteerRequestDto(
            volunteerRequest.Id,
            volunteerRequest.AdminId,
            volunteerRequest.UserId,
            volunteerRequest.VolunteerInfo,
            volunteerRequest.Status.ToString(),
            volunteerRequest.CreatedAt.Value,
            volunteerRequest.RejectedComment,
            volunteerRequest.DiscussionId);
         
        return volunteerRequestDto;
    }
     
}
