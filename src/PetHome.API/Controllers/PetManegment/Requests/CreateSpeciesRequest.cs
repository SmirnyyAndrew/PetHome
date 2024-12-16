using PetHome.Application.Features.Volunteers.PetManegment.CreateSpeciesVolunteer;

public record CreateSpeciesRequest(Guid SpeciesId, string BreedName)
{
    public static implicit operator CreateSpeciesCommand(CreateSpeciesRequest request)
    {
        return new CreateSpeciesCommand(request.SpeciesId, request.BreedName);
    } 
}
