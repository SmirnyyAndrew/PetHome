using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.MainInfo;

public record FullName
{
    public const int MAX_NAME_LENGTH = 20;
    public string FirstName { get; }
    public string LastName { get; }

    private FullName() { }
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<FullName, Error> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            return Errors.Validation("Имя и фамилия");

        return new FullName(firstName, lastName);
    }
}
