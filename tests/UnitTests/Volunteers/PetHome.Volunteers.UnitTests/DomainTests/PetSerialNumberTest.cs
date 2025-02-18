using CSharpFunctionalExtensions;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using Xunit;
namespace PetHome.Volunteers.UnitTests.DomainTests;

public class PetSerialNumberTest
{
    [Fact]
    public void Execute()
    {
        int petCount = 20;

        for (int i = 0; i < petCount; i++)
        {
            List<Requisites> requisites = new List<Requisites>() { Requisites.Create("name", "desc", PaymentMethodEnum.Cash).Value };

            Pet pet = Pet.Create(
                PetName.Create(i.ToString()).Value,
                SpeciesId.Create().Value,
                Description.Create("Описание").Value,
                BreedId.Create().Value,
                Color.Create("Чёрный").Value,
                PetShelterId.Create().Value,
                23,
                false,
                Date.Create(DateTime.Parse("10.10.2024")).Value,
                false,
                PetStatusEnum.isHomed,
                VolunteerId.CreateEmpty().Value,
                requisites)
                .Value;
        }

        //Проверка инициализации и уникальности serial number
        Assert.Equal(GetUniqueSerialNumbersCount(), petCount);
        CheckOrderAssert(petCount);


        //Проверка переноса serial number вначало, уникальности всех номеров и их порядок
        int numToCheck = 5;
        string nameWithOldSerialNumber = Pet.Pets[numToCheck].Name.Value;
        Pet.Pets[numToCheck].ChangeSerialNumberToBeginning();
        string nameWithNewSerialNumber = Pet.Pets[0].Name.Value;

        Assert.Equal(GetUniqueSerialNumbersCount(), petCount);
        CheckOrderAssert(petCount);
        Assert.Equal(nameWithOldSerialNumber, nameWithNewSerialNumber);


        //Проверка переноса serial number НА УБЫВАНИЕ, уникальности всех номеров и их порядок
        numToCheck = 18;
        int newSerialNumber = 12;
        nameWithOldSerialNumber = Pet.Pets[numToCheck].Name.Value;
        Pet.Pets[numToCheck].ChangeSerialNumber(newSerialNumber);
        nameWithNewSerialNumber = Pet.Pets[newSerialNumber - 1].Name.Value;

        Assert.Equal(GetUniqueSerialNumbersCount(), petCount);
        CheckOrderAssert(petCount);
        Assert.Equal(nameWithOldSerialNumber, nameWithNewSerialNumber);


        //Проверка переноса serial number НА ВОЗРАСТАНИЕ, уникальности всех номеров и их порядок
        numToCheck = 11;
        newSerialNumber = 20;
        nameWithOldSerialNumber = Pet.Pets[numToCheck].Name.Value;
        Pet.Pets[numToCheck].ChangeSerialNumber(newSerialNumber);
        nameWithNewSerialNumber = Pet.Pets[newSerialNumber - 1].Name.Value;

        Assert.Equal(GetUniqueSerialNumbersCount(), petCount);
        CheckOrderAssert(petCount);
        Assert.Equal(nameWithOldSerialNumber, nameWithNewSerialNumber);


    }

    private int GetUniqueSerialNumbersCount() => Pet.Pets.Select(x => x.SerialNumber.Value).Distinct().Count();

    private void CheckOrderAssert(int petsCount)
    {
        for (int i = 0; i < petsCount; i++)
        {
            Assert.Equal(Pet.Pets[i].SerialNumber.Value, i + 1);
        }
    }
}
