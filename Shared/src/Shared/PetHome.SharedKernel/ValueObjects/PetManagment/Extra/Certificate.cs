using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
public record Certificate
{
    public string Name { get; }
    public string Value { get; }

    private Certificate() { }
    private Certificate(string name, string description)
    {
        Name = name;
        Value = description;
    }

    public static Result<Certificate, Error> Create(string name, string description)
    {
        bool isInvalidRequisite = string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description);

        if (isInvalidRequisite)
            return Errors.Validation("Реквезиты");

        return new Certificate(name, description);
    }
}
