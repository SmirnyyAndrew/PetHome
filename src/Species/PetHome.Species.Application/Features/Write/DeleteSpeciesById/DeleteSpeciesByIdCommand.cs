
namespace PetHome.Species.Application.Features.Write.DeleteSpeciesById;
public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;
