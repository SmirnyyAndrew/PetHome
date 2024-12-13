using PetHome.Application.Features.Dtos.Volunteer;

namespace PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerRequest(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto);

public record UpdateMainInfoVolunteerDto(
    FullNameDto FullNameDto,
    string Description,
    IEnumerable<string> PhoneNumbers,
    string Email);
