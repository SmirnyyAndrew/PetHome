using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Volunteers.Domain.PetManagment.PetEntity;
public class PetShelter
{
    private PetShelter() { } 
    private PetShelter(string value)
    {
        Id = PetShelterId.Create().Value;
        Name = ShelterName.Create(value).Value;
    }

    public PetShelterId Id { get; private set; }
    public ShelterName Name { get; private set; }

    public static Result<PetShelter, Error> Create(string value)
    {
        return new PetShelter(value);
    }
}
