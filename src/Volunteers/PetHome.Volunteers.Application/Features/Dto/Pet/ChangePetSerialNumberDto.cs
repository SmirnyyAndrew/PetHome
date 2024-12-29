namespace PetHome.Volunteers.Application.Features.Dto.Pet;

public record ChangePetSerialNumberDto(
    Guid PetId,
    int NewSerialNumber);
