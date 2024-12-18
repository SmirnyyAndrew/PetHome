using PetHome.Application.Features.Volunteers.PetManegment.CreateSpecies;

public record CreateSpeciesRequest(string SpeciesName)
{
    public static implicit operator CreateSpeciesCommand(CreateSpeciesRequest request)
    {
        return new CreateSpeciesCommand(request.SpeciesName);
    } 
}
