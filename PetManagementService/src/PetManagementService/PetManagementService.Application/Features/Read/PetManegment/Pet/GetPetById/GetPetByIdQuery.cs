using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Read.PetManegment.Pet.GetPetById;
public record GetPetByIdQuery(
  Guid PetId  ) : IQuery;
