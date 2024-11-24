using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetEntity;
using System.Drawing;

namespace PetHome.Domain.VolunteerEntity
{
    public class Volunteer
    {
        //Для EF core
        private Volunteer() { }



        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string? Email { get; private set; }
        public string Description { get; private set; }
        public DateOnly StartVolunteeringDate { get; private set; }



        public int HomedPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isHomed);
        public int FreePetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isFree);
        public int TreatmentPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isTreatment);


        public PhoneNumber PhoneNumber { get; private set; }
        public List<SocialNetwork> SocialNetworkList { get; private set; }
        public Requisites Requisites { get; private set; }





        private int GetPetCountByStatusAndVolunteer(PetStatusEnum status) => Pet.GetPetList().Where(p => p.Status == status && p.Volunteer.Id == Id).Count();




        public Result<Volunteer> Create(string fullName, string email, string description, DateOnly startVolunteeringDate,
            PhoneNumber phoneNumber, List<SocialNetwork> socialNetworkList, Requisites requisites)
        {

            if (string.IsNullOrWhiteSpace(fullName))
                return Result.Failure<Volunteer>("Введите имя");


            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Volunteer>("Введите описание");
             

            if (startVolunteeringDate == null)
                return Result.Failure<Volunteer>("Выберите дату начала волонтёрства");





            return new Volunteer()
            {
                FullName = fullName,
                Description = description,
                PhoneNumber = phoneNumber,
                Email = email,
                Requisites = requisites,
                SocialNetworkList = socialNetworkList,
                StartVolunteeringDate = startVolunteeringDate
            };

        }
    }
}
