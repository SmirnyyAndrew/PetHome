using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PetHome.API.Response;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using Xunit;
using Color = PetHome.Domain.PetManagment.PetEntity.Color;

namespace PetHome.UnitTests.DomainTests;

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
                Domain.PetManagment.GeneralValueObjects.Date.Create(DateTime.Parse("10.10.2024")).Value,
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
        Pet.Pets[numToCheck].ChangeSerialNumberToBegining();
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
