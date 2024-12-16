namespace PetHome.Application.Features.Volunteers.PetManegment.CreateBreedVolunteer;

public record CreateBreedRequst(Guid SpeciesId, IEnumerable<string> Breeds)
{
    public static implicit operator CreateBreedCommand(CreateBreedRequst requst)
    {
        return new CreateBreedCommand(requst.SpeciesId, requst.Breeds);
    }
}