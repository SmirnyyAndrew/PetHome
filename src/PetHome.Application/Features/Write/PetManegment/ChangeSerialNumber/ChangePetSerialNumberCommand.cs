using PetHome.Application.Features.Dtos.Pet;
using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.PetManegment.ChangeSerialNumber;
public record ChangePetSerialNumberCommand(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto) : ICommand;
