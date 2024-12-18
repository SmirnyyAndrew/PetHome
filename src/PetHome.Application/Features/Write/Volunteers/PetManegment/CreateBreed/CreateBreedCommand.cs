namespace PetHome.Application.Features.Write.Volunteers.PetManegment.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, IEnumerable<string> Breeds);