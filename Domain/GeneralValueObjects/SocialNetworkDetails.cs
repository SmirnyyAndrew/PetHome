namespace PetHome.Domain.GeneralValueObjects
{
    public record SocialNetworkDetails 
    {
        public IReadOnlyList<SocialNetwork> Values { get; }
         
        private SocialNetworkDetails() { }
        public SocialNetworkDetails(IReadOnlyList<SocialNetwork> values)
        {
            Values = values;
        }

        public static SocialNetworkDetails Create(List<SocialNetwork> values)
        {
            return new SocialNetworkDetails(values);
        }
    }
}