using PetHome.Application.Features.Write.VolunteerManegment.UpdateMainInfoVolunteer;

public record UpdateMainInfoVolunteerRequest(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto)
{
    public static implicit operator UpdateMainInfoVolunteerCommand(UpdateMainInfoVolunteerRequest request)
    {
        UpdateMainInfoVolunteerDto dto = new UpdateMainInfoVolunteerDto(
            request.UpdateMainInfoDto.FullNameDto,
            request.UpdateMainInfoDto.Description,
            request.UpdateMainInfoDto.PhoneNumbers,
            request.UpdateMainInfoDto.Email);

        return new UpdateMainInfoVolunteerCommand(
            request.Id, dto);
    }
} 