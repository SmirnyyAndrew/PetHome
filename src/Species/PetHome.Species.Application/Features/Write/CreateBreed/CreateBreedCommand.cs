using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.PetManegment.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, IEnumerable<string> Breeds) : ICommand;