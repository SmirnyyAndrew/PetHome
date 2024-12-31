using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Volunteers.Application.Features.Dto.Pet;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.ChangeSerialNumber;
public record ChangePetSerialNumberCommand(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto) : ICommand;
