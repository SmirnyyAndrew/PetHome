namespace PetHome.Application.Features.Dtos.Pet;

public record ChangePetSerialNumberDto(
    Guid PetId,
    int NewSerialNumber);
