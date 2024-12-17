namespace PetHome.Application.Features.Volunteers.PetManegment.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, IEnumerable<string> Breeds);