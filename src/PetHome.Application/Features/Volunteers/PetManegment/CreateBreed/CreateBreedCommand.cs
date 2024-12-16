namespace PetHome.Application.Features.Volunteers.PetManegment.CreateBreedVolunteer;

public record CreateBreedCommand(Guid SpeciesId, IEnumerable<string> Breeds);