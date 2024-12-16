namespace PetHome.Application.Features.Volunteers.PetManegment.CreateBreedVolunteer;

public record CreateBreedRequst(Guid SpeciesId, IEnumerable<string> Breeds);