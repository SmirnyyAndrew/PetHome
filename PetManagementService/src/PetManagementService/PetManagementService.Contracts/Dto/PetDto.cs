namespace PetManagementService.Contracts.Dto;
public record PetDto(
   Guid Id,
   string Name,
   Guid SpeciesId,
   string Description,
   Guid? BreedId,
   string Color,
   Guid ShelterId,
   double Weight,
   bool IsCastrated,
   DateTime? BirthDate,
   bool IsVaccinated,
   string Status,
   Guid VolunteerId,
   int SerialNumber);

