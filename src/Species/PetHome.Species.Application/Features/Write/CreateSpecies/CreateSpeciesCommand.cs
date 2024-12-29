using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Write.PetManegment.CreateSpecies;
public record CreateSpeciesCommand(string SpeciesName) : ICommand;
