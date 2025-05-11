namespace PetManagementService.Application.Dto.Pet;

public record ChangePetSerialNumberDto(
    Guid PetId,
    int NewSerialNumber);
