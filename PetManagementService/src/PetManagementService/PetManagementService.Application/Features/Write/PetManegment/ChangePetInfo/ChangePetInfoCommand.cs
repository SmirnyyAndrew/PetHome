using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetManagementService.Application.Features.Write.PetManegment.ChangePetInfo;
public record ChangePetInfoCommand(
        Guid PetId,
        string Name,
        Guid SpeciesId,
        string Description,
        Guid BreedId,
        string Color,
        Guid ShelterId,
        double Weight,
        bool IsCastrated,
        DateTime BirthDate,
        bool IsVaccinated,
        PetStatusEnum Status,
        Guid VolunteerId,
        IReadOnlyList<RequisitesesDto> Requisiteses) : ICommand;
