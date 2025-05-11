using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, IEnumerable<string> Breeds) : ICommand;