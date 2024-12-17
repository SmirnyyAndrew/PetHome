namespace PetHome.Application.Features.Volunteers.VolunteerManegment.CreateVolunteer;

public record CreateVolunteerCommand(
        FullNameDto FullNameDto,
        string Email,
        string Description,
        DateTime StartVolunteeringDate,
        IEnumerable<string> PhoneNumbers,
        IEnumerable<string> SocialNetworks,
        IEnumerable<RequisitesesDto> RequisitesesDto);

