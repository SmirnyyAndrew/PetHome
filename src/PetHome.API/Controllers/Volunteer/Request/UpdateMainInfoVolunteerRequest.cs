using PetHome.Application.Features.Dtos.Volunteer;
using PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;

public record UpdateMainInfoVolunteerRequest(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto)
{
    public static implicit operator UpdateMainInfoVolunteerCommand(UpdateMainInfoVolunteerRequest request)
    {
        PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer.UpdateMainInfoVolunteerDto dto =
            new(request.UpdateMainInfoDto.FullNameDto,
            request.UpdateMainInfoDto.Description,
            request.UpdateMainInfoDto.PhoneNumbers,
            request.UpdateMainInfoDto.Email);

        return new UpdateMainInfoVolunteerCommand(
            request.Id, dto);
    }
}

public record UpdateMainInfoVolunteerDto(
    FullNameDto FullNameDto,
    string Description,
    IEnumerable<string> PhoneNumbers,
    string Email);
