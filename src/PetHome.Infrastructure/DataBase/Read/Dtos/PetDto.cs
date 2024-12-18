using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Interfaces;

public class PetDto : SoftDeletableEntity
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid SpeciesId { get; init; }
    public string Description { get; init; }
    public Guid? BreedId { get; init; }
    public string Color { get; init; }
    public Guid ShelterId { get; init; }
    public double Weight { get; init; }
    public bool IsCastrated { get; init; }
    public DateTime? BirthDate { get; init; }
    public bool IsVaccinated { get; init; }
    public PetStatusEnum Status;
    //public RequisitesesDto[] Requisites { get; init; }
    public DateTime ProfileCreateDate { get; init; }
    public Guid VolunteerId { get; init; }
    public int SerialNumber { get; init; }
    //public MediaDto[] Medias { get; init; }
}
