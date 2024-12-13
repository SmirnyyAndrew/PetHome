using PetHome.Application.Features.Dtos.Pet;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreatePetVolunteer;
public record CreatePetRequest(Guid VolunteerId, PetMainInfoDto PetMainInfoDto);