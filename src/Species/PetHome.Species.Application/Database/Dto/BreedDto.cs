namespace PetHome.Application.Database.Dtos;
public class BreedDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid SpeciesId { get; init; }
}
