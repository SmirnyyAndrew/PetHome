using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces.Database;
using PetHome.Core.Models;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Species;

namespace PetHome.Species.Domain.SpeciesManagment.BreedEntity;
public class Breed : DomainEntity<BreedId>, ISoftDeletableEntity
{
    private Breed(BreedId id) : base(id) { }
    private Breed(BreedName name, SpeciesId speciesId)
        : base(BreedId.Create().Value)
    {
        Name = name;
        SpeciesId = speciesId;
    }

    public BreedId Id { get; private set; }
    public BreedName Name { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
    public DateTime DeletionDate { get; set; }
    public bool IsDeleted { get; set; }

    public static Result<Breed, Error> Create(string name, Guid speciesId)
    {
        var nameResult = BreedName.Create(name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        return new Breed(
            nameResult.Value,
            SpeciesId.Create(speciesId).Value);
    }


    public void SoftDelete()
    {
        DeletionDate = DateTime.UtcNow;
        IsDeleted = true;
    }

    public void SoftRestore()
    {
        DeletionDate = default;
        IsDeleted = false;
    }
}
