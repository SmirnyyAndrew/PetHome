using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetStatus;
public record ChangePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    PetStatusEnum NewPetStatus) : ICommand;
