using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.PetManegment.DeleteSpeciesById;
public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;
