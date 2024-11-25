namespace PetHome.Domain.GeneralValueObjects;
public class SocialNetwork
{
    // Для EF core
    private SocialNetwork() { }

    public int Id { get; private set; }
    public string Url { get; private set; }
}
