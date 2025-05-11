namespace PetManagementService.Application.Database.Dto;
public class BreedDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid SpeciesId { get; init; }
}
