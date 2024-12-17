namespace PetHome.Infrastructure.DataBase.Read.Dtos;
public class BreedDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid SpeciesId { get; init; }
}
