namespace PetHome.Application.Features.Volunteers.CreateBreedVolunteer;

public record  VolunteerCreateBreedRequst(Guid SpeciesId, IEnumerable<string> Breeds);