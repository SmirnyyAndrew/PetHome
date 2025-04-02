using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateSpecies;
public record CreateSpeciesCommand(string SpeciesName) : ICommand;
