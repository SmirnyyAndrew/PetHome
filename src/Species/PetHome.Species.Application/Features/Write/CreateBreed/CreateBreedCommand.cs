
namespace PetHome.Species.Application.Features.Write.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, IEnumerable<string> Breeds) : ICommand;