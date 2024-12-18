using PetHome.Application.Features.Dtos.Pet;

namespace PetHome.Application.Features.Write.Volunteers.PetManegment.ChangeSerialNumber;
public record ChangePetSerialNumberCommand(
    Guid VolunteerId,
    ChangePetSerialNumberDto ChangeNumberDto);
