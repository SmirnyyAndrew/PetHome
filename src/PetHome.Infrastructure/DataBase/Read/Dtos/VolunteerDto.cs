using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Interfaces;

public class VolunteerDto : SoftDeletableEntity
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string? Email { get; private set; }
    public string Description { get; private set; }
    public DateTime StartVolunteeringDate { get; private set; }
    public PetDto[] Pets { get; private set; }
    public int HomedPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isHomed);
    public int FreePetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isFree);
    public int TreatmentPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isTreatment);
    public string[] PhoneNumbers { get; private set; }
    public RequisitesesDto[] Requisites { get; private set; }
    public SocialNetworkDto[] SocialNetworks { get; private set; }

    private int GetPetCountByStatusAndVolunteer(PetStatusEnum status)
        => Pets.Count(pet => pet.Status == status && pet.VolunteerId == Id);
}
