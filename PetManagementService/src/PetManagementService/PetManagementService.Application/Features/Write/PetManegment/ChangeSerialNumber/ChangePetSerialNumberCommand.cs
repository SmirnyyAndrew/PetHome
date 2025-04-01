using PetHome.Core.Interfaces.FeatureManagment;
using PetManagementService.Application.Dto.Pet;

namespace PetManagementService.Application.Features.Write.PetManegment.ChangeSerialNumber;
public record ChangePetSerialNumberCommand(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto) : ICommand;
