using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.SharedKernel.ValueObjects.MainInfo;
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
