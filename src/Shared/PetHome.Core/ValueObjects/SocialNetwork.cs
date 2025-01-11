using CSharpFunctionalExtensions;
using PetHome.Core.Response.ErrorManagment;

namespace PetHome.Core.ValueObjects;
public record SocialNetwork
{
    public string Value { get; }

    private SocialNetwork() { }
    private SocialNetwork(string value)
    {
        Value = value;
    } 

    public static Result<SocialNetwork, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.Validation("Социальная сеть");

        return new SocialNetwork(value);
    }
}
