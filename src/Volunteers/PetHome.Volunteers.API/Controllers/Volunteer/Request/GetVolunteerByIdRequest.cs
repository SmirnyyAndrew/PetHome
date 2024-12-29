using PetHome.Application.Features.Read.VolunteerManegment.GetVolunteerById;

namespace PetHome.API.Controllers.Volunteer.Request;

public record GetVolunteerByIdRequest(Guid VolunteerId)
{
    public static implicit operator GetVolunteerByIdQuery(GetVolunteerByIdRequest request)
    {
        return new GetVolunteerByIdQuery(request.VolunteerId);
    }
}