using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Accounts.Domain.Aggregates.User.Accounts;
public class VolunteerAccount : SoftDeletableEntity
{
    public UserId? UserId { get; private set; }
    public Date StartVolunteeringDate { get; private set; }
    public ValueObjectList<Requisites> Requisites { get; private set; }
    public ValueObjectList<Certificate> Certificates { get; private set; }
    public List<Pet> Pets { get; private set; } = new List<Pet>(); 
    
    private VolunteerAccount() { }
    private VolunteerAccount(
        UserId userId,
        Date startVolunteeringDate)
    {
        UserId = userId;
        StartVolunteeringDate = startVolunteeringDate;
    }

    public static VolunteerAccount Create(
        UserId? userId,
            Date startVolunteeringDate,
            ValueObjectList<Requisites> requisites,
            ValueObjectList<Certificate> certificates)
    {
        return new VolunteerAccount(userId, startVolunteeringDate);
    } 
}
