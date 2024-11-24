using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Domain.PetEntity
{
    public class Pet
    {
        //Для EF core
        private Pet() { }



        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Species Species;
        public string Description { get; private set; }
        public Breed Breed;
        public Color Color;
        public PetShelter Address { get; private set; }
        public double Weight { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public bool IsCastrated { get; private set; }
        public BirthDate? BirthDate { get; private set; }
        public bool IsVaccinated { get; private set; }

        public PetStatusEnum Status;
        public Requisites? Requisites { get; private set; }
        public DateTime ProfileCreateDate { get; private set; }

        public Volunteer Volunteer { get; private set; }



        // TODO: реализовать метод после интеграции БД
        public static List<Pet> GetPetList() => new List<Pet>();



        public static Result<Pet> Create(string name, Species species, string description, Breed breed, Color color, PetShelter address, double weight,
            PhoneNumber phoneNumber, bool isCastrated, BirthDate birthDate, bool isVaccinated, PetStatusEnum status, Requisites requisites, DateTime profileCreateDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Pet>("Введите имя");


            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Pet>("Введите описание");


            if (color == null)
                return Result.Failure<Pet>("Выберите цвет животного");


            if (weight > 500 || weight <= 0)
                return Result.Failure<Pet>("Введите корректный вес");





            return new Pet()
            {
                Name = name,
                Species = species,
                Description = description,
                Breed = breed,
                Color = color,
                Address = address,
                Weight = weight,
                PhoneNumber = phoneNumber,
                IsCastrated = isCastrated,
                BirthDate = birthDate,
                IsVaccinated = isVaccinated,
                Status = status,
                Requisites = requisites,
                ProfileCreateDate = profileCreateDate
            };
        }

    }






    #region PetValueObjects



    public class Color : ValueObject
    {
        private string Name { get; }

        private Color(string name)
        {
            Name = name;
        }

        public Result<Color> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Color>("Введите цвет");

            return new Color(name);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }



    public class BirthDate : ValueObject
    {
        private DateOnly Date { get; }

        private BirthDate(DateOnly date)
        {
            Date = date;
        }

        public Result<BirthDate> Create(DateOnly date)
        {
            if (date == null || date.Year > 100)
            {
                return Result.Failure<BirthDate>("Введите корректную дату");
            }


            return new BirthDate(Date);
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }
    }


    /*
    public class PetStatus : ValueObject
    {
        public static readonly PetStatus isNeedHelp = new PetStatus("Нужна помощь");
        public static readonly PetStatus isThreatment = new PetStatus("На лечении");
        public static readonly PetStatus isHomed = new PetStatus("Нашёл дом");

        private static PetStatus[] allStatus = {isNeedHelp, isThreatment, isHomed};


        private string StatusValue { get; }

        private PetStatus(string status)
        {
            StatusValue = status;
        }


        public Result<PetStatus> Create(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return Result.Failure<PetStatus>("Введите статус животного");


            if(!allStatus.Any(x=>x.StatusValue.ToLower() == status.ToLower()))
            {
                return Result.Failure<PetStatus>("Введенное значение некорректно");
            }



            return new PetStatus(status);
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StatusValue;
        }
    }

    */

    #endregion
}
