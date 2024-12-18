namespace PetHome.Application.Features.Write.Volunteers.VolunteerManegment.UpdateMainInfoVolunteer;
public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    UpdateMainInfoVolunteerDto UpdateMainInfoDto);
