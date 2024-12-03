namespace PetHome.Domain.GeneralValueObjects;
public record SocialNetworkDetails 
{
    public IReadOnlyList<SocialNetwork> Values { get; }
     
    private SocialNetworkDetails() { }
    public SocialNetworkDetails(IEnumerable<SocialNetwork> values)
    {
        Values = values.ToList();
    }

    public static SocialNetworkDetails Create(IEnumerable<SocialNetwork> values)
    {
        return new SocialNetworkDetails(values.ToList());
    }
}