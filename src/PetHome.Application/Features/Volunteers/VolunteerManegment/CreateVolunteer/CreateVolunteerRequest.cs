using PetHome.Application.Features.Dtos.Volunteer;

namespace PetHome.Application.Features.Volunteers.VolunteerManegment.CreateVolunteer;

public record CreateVolunteerRequest(
        FullNameDto FullNameDto,
        string Email,
        string Description,
        DateTime StartVolunteeringDate,
        IEnumerable<string> PhoneNumbers,
        IEnumerable<string> SocialNetworks,
        IEnumerable<RequisitesesDto> RequisitesesDto);

