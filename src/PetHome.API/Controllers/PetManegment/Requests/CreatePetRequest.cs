using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Features.Volunteers.PetManegment.CreatePet;

public record CreatePetRequest(Guid VolunteerId, PetMainInfoDto PetMainInfoDto)
{
    public static implicit operator CreatePetCommand(CreatePetRequest request)
    {
        return new CreatePetCommand(request.VolunteerId, request.PetMainInfoDto);
    }
}