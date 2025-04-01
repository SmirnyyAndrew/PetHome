
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.PetManegment.DeleteSpeciesById;
public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;
