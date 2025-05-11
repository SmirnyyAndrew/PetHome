using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetManagementService.Domain.PetManagment.PetEntity;
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
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Название приюта");

        return new PetShelter(value);
    }
}
