using CSharpFunctionalExtensions; 

namespace PetHome.Domain.PetEntity
{
    public record BreedId 
    {
        public Guid Value { get; }
        private BreedId(Guid value)
        {
            Value = value;
        }

        public static BreedId Create() => new BreedId(Guid.NewGuid());

        public static BreedId Create(Guid id) => new BreedId(id);

        public static BreedId CreateEmpty() => new BreedId(Guid.Empty);

        public static implicit operator Guid(BreedId breedId)
        {
            if (breedId == null)
                throw new ArgumentNullException();

            return breedId.Value;
        }
    }
}