using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHome.Domain.PetEntity
{
    public class PetShelter
    {
        // Для EF core
        private PetShelter() { }


        public int Id { get; private set; }
        public ShelterName Name { get; private set; }
    }

    public class ShelterName : ValueObject
    {
        private string Name { get; }

        private ShelterName(string name)
        {
            Name = name;
        }

        public Result<ShelterName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<ShelterName>("Строка не должна быть пустой");

            return new ShelterName(name);
        }



        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }

    }

}
