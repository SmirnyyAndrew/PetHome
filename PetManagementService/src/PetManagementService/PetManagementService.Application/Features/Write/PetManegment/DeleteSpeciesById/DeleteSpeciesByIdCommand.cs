
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.PetManegment.DeleteSpeciesById;
public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;
