namespace PetHome.Volunteers.Application.Dto.Pet;

public record ChangePetSerialNumberDto(
    Guid PetId,
    int NewSerialNumber);
