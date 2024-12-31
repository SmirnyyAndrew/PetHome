
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Species.Application.Features.Write.CreateSpecies;
public record CreateSpeciesCommand(string SpeciesName) : ICommand;
