using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Write.SpeciesManagement.CreateSpecies;
public record CreateSpeciesCommand(string SpeciesName) : ICommand;
