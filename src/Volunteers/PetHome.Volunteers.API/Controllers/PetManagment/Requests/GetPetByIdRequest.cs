using PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetById;

namespace PetHome.Volunteers.API.Controllers.PetManagment.Requests;

public record GetPetByIdRequest(Guid PetId)
{
    public static implicit operator GetPetByIdQuery(GetPetByIdRequest request)
    {
        return new GetPetByIdQuery(request.PetId);
    }
}
