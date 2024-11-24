using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity
{
    public class Breed
    {
        // Для EF core
        private Breed() { }
         

        public int Id { get; private set; }
        public BreedName Name { get; private set; }  
    }


    #region ValueObjects

    public class BreedName : ValueObject
    {
        private string Name { get; }

        private BreedName(string name)
        {
            Name = name;
        }

        public Result<BreedName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<BreedName>("Строка не должна быть пустой");

            return new BreedName(name);
        }



        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }

    }


    #endregion
}
