using CSharpFunctionalExtensions;

namespace PetHome.Domain.PetEntity
{
    public record PetName
    {
        public string Value { get;}
        private PetName(string value)
        {
            Value = value;
        }

        public static Result<PetName> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<PetName>("Введите имя");

            return new PetName(value);
        }
    }
}