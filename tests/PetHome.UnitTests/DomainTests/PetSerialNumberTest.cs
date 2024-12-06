using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using System.Drawing;
using System.Xml.Linq;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Color = PetHome.Domain.PetManagment.PetEntity.Color;

namespace PetHome.UnitTests.DomainTests;

public class PetSerialNumberTest
{
    [Fact]
    public void Execute()
    {
        int petsCount = 20;

        for (int i = 0; i < petsCount; i++)
        {
            Pet pet = Pet.Create(
                PetId.Create(),
                PetName.Create(i.ToString()).Value,
                SpeciesId.Create().Value,
                Description.Create("Описание").Value,
                BreedId.Create(),
                Color.Create("Чёрный").Value,
                PetShelterId.Create(),
                23,
                false,
                Domain.PetManagment.GeneralValueObjects.Date.Create(DateOnly.Parse("10.10.2024")).Value,
                false,
                PetStatusEnum.isHomed,
                RequisitesDetails.Create(new List<Requisites>() { Requisites.Create("name", "desc", PaymentMethodEnum.Cash).Value }).Value,
                Domain.PetManagment.GeneralValueObjects.Date.Create(DateOnly.Parse("10.10.2024")).Value).Value;
        }

        //Проверка инициализации и уникальности serial number
        Assert.Equal(GetUniqueSerialNumbersCount(), petsCount);
        CheckOrderAssert(petsCount);


        //Проверка переноса serial number вначало, уникальности всех номеров и их порядок
        int numToCheck = 5;
        string nameWithOldSerialNumber = Pet.Pets[numToCheck].Name.Value;
        Pet.Pets[numToCheck].ChangeSerialNumberToBegining();
        string nameWithNewSerialNumber = Pet.Pets[0].Name.Value;

        Assert.Equal(GetUniqueSerialNumbersCount(), petsCount);
        CheckOrderAssert(petsCount);
        Assert.Equal(nameWithOldSerialNumber, nameWithNewSerialNumber);


        //Проверка переноса serial number на n-ую позицию, уникальности всех номеров и их порядок
        numToCheck = 18;
        int newSerialNumber = 12;
        nameWithOldSerialNumber = Pet.Pets[numToCheck].Name.Value;
        Pet.Pets[numToCheck].ChangeSerialNumber(newSerialNumber);
        nameWithNewSerialNumber = Pet.Pets[newSerialNumber - 1].Name.Value;

        Assert.Equal(GetUniqueSerialNumbersCount(), petsCount);
        CheckOrderAssert(petsCount);
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
