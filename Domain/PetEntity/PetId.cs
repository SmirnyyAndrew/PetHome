using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity
{
    public record class PetId
    {  
        public Guid Value { get; }
        
        private PetId() { }
        private PetId(Guid value)
        {
            Value = value;
        }

        public static PetId Create() => new PetId(Guid.NewGuid()); 
                
        public static PetId Create(Guid id) => new PetId(id); 
                
        public static PetId CreateEmpty() => new PetId(Guid.Empty); 

        public static implicit operator Guid(PetId petId)
        {
            if (petId == null)
                throw new ArgumentNullException();

            return petId.Value;
        }
    }
}