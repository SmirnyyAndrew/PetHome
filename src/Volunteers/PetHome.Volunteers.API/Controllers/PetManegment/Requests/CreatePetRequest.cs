using PetHome.Volunteers.Application.Features.Dto.Pet;
using PetHome.Volunteers.Application.Features.Write.PetManegment.CreatePet;

public record CreatePetRequest(Guid VolunteerId, PetMainInfoDto PetMainInfoDto)
{
    public static implicit operator CreatePetCommand(CreatePetRequest request)
    {
        return new CreatePetCommand(request.VolunteerId, request.PetMainInfoDto);
    }
}