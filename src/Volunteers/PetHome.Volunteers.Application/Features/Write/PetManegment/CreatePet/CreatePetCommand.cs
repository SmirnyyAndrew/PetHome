using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Dto.Pet;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.CreatePet;
public record CreatePetCommand(Guid VolunteerId, PetMainInfoDto PetMainInfoDto) : ICommand;