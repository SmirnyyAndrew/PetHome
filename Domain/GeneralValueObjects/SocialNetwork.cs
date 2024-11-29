namespace PetHome.Domain.GeneralValueObjects;
public record SocialNetwork
{ 
    private SocialNetwork() { }

    public string Url { get; private set; }
}
