using PetHome.Core.Interfaces.MessageBusManagement;
using PetHome.Core.ValueObjects.PetManagment.Pet;

namespace PetManagementService.Domain.Events;
public record CreatedPetEvent(PetId PetId) : IDomainEvent;
