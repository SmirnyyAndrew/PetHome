using PetHome.Infrastructure.DataBase.Read.Dtos;

public class SpeciesDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public BreedDto[] Breeds { get; init; }
}
