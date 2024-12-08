using PetHome.Application.Features.Volunteers.VolunteerDtos;

namespace PetHome.Application.Features.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
        FullNameDto FullNameDto,
        string Email,
        string Description,
        DateTime StartVolunteeringDate,
        IEnumerable<string> PhoneNumbers,
        IEnumerable<string> SocialNetworks,
        IEnumerable<RequisitesesDto> RequisitesesDto);

