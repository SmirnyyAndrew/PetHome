using CSharpFunctionalExtensions;

namespace PetHome.Domain.GeneralValueObjects;
public record SocialNetwork
{
    public string Value { get; }

    private SocialNetwork() { }
    private SocialNetwork(string value)
    {
        Value = value;
    }
    public string Url { get; private set; }

    public static Result<SocialNetwork> Create(string value) => new SocialNetwork(value);
}
