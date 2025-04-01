using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, IEnumerable<string> Breeds) : ICommand;