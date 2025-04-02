using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.User;
public record UserName
{
    public string Value { get; }
    private UserName(string value)
    {
        Value = value;
    }

    public static Result<UserName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("UserName");

        return new UserName(value);
    }

    public static implicit operator string(UserName name) => name.Value;
}
