namespace PetHome.Application.Features.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto);
