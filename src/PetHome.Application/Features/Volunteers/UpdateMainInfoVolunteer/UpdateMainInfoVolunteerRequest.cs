using PetHome.Application.Features.Volunteers.VolunteerDtos;

namespace PetHome.Application.Features.Volunteers.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerRequest(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto);

public record UpdateMainInfoVolunteerDto(
    FullNameDto FullNameDto,
    string Description,
    IEnumerable<string> PhoneNumbers,
    string Email);
