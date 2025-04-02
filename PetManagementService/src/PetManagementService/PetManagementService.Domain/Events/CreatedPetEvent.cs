using PetHome.Core.Application.Interfaces.MessageBusManagement;
using PetManagementService.Domain.PetManagment.PetEntity;

namespace PetManagementService.Domain.Events;
public record CreatedPetEvent(PetId PetId) : IDomainEvent;
