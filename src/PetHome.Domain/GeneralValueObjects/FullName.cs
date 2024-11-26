using PetHome.Domain.VolunteerEntity;

namespace PetHome.Domain.GeneralValueObjects
{
    public record FullName
    {
        public const int MAX_NAME_LENGTH = 20;

        public string FirstName { get; }
        public string LastName { get; }
        private FullName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static FullName Create(string firstName, string lastName) => new FullName(firstName, lastName);

    }
}
