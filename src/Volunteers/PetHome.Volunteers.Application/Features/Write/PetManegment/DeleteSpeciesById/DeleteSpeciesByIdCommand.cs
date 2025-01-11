
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.DeleteSpeciesById;
public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;
