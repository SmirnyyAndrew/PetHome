using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Domain.PetManagment.PetEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.ChangePetStatus;
public record ChangePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    PetStatusEnum NewPetStatus) : ICommand;
