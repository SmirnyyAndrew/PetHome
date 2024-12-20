using PetHome.Application.Features.Dtos.Pet;

namespace PetHome.Application.Features.Write.PetManegment.ChangeSerialNumber;
public record ChangePetSerialNumberCommand(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto);
