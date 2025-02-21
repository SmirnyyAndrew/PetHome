using PetHome.Core.Interfaces.MessageBusManagement;
using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetHome.Volunteers.Domain.Events;
public record CreatedPetEvent(PetId PetId):IDomainEvent;
