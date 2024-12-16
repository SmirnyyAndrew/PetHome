using PetHome.Application.Features.Dtos.Pet;

namespace PetHome.Application.Features.Volunteers.PetManegment.CreatePetVolunteer;
public record CreatePetCommand(Guid VolunteerId, PetMainInfoDto PetMainInfoDto);